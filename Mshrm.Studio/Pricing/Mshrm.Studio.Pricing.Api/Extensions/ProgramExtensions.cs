﻿using AspNetCoreRateLimit;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mshrm.Studio.Shared.Builders;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Mshrm.Studio.Shared.Helpers;
using Mshrm.Studio.Shared.Models.Options;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Providers;
using Mshrm.Studio.Pricing.Api.Models.Options;
using Mshrm.Studio.Pricing.Api.Mapping;
using Mshrm.Studio.Pricing.Api.Services.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Mshrm.Studio.Pricing.Api.Services.Jobs.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Jobs;
using Mshrm.Studio.Pricing.Api.Services.Http.Interfaces;
using Mshrm.Studio.Pricing.Api.Context;
using Mshrm.Studio.Pricing.Api.Repositories;
using Mshrm.Studio.Pricing.Api.Services.Http;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mshrm.Studio.Shared.Models.Pagination;
using Mshrm.Studio.Pricing.Api.Models.CQRS.AssetPrices.Queries;
using Mshrm.Studio.Pricing.Api.Models.CQRS.AssetPrices.Commands;
using Mshrm.Studio.Pricing.Api.Models.CQRS.AssetPriceHistories.Commands;
using Mshrm.Studio.Pricing.Application.Handlers.Request.Assets;
using Google.Api;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Services.Providers.Interfaces;
using Mshrm.Studio.Pricing.Application.Services.Providers;
using System.Data.SqlClient;
using Mshrm.Studio.Pricing.Infrastructure.Factories;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Assets.Commands;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Assets.Queries;
using Mshrm.Studio.Pricing.Application.Handlers.Request.Assets;
using Mshrm.Studio.Pricing.Domain.Assets;
using Mshrm.Studio.Pricing.Application.Services.Background;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Application.Handlers.Request.Providers;
using Mshrm.Studio.Pricing.Domain.ProviderAssets.Queries;
using Mshrm.Studio.Pricing.Domain.ProviderAssets;
using Mshrm.Studio.Shared.Services.Interfaces;
using Mshrm.Studio.Shared.Services;
using Microsoft.Extensions.Caching.Distributed;
using Mshrm.Studio.Pricing.Domain.AssetPrices.Queries;
using Mshrm.Studio.Pricing.Domain.AssetPriceHistories.Queries;
using Mshrm.Studio.Pricing.Domain.AssetPriceHistories;
using Mshrm.Studio.Pricing.Application.Handlers.Request.AssetPriceHistories;
using Mshrm.Studio.Pricing.Application.Handlers.Request.AssetPrices;
using Mshrm.Studio.Pricing.Domain.AssetPrices;
using Mshrm.Studio.Pricing.Api.Models.CQRS.AssetPrices.Events;
using Mshrm.Studio.Pricing.Api.EventHandlers.AssetPrices;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;

namespace Mshrm.Studio.Pricing.Api.Extensions
{
    /// <summary>
    /// Extensions for program.cs
    /// </summary>
    public static class ProgramExtensions
    {
        /// <summary>
        /// Add CORS to API (restrict front-end access)
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors();

            return builder;
        }

        /// <summary>
        /// Add application settings + environment variables - environment specific
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureSettings(this WebApplicationBuilder builder)
        {
            // Base app settings is loaded
            builder.Configuration.AddJsonFile("appsettings.json", false, true);

            // Environment dependant settings
            if (builder.Environment.IsDevelopment() && Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") != "true")
                builder.Configuration.AddJsonFile("appsettings.Development.json", false, true);
            if (builder.Environment.IsDevelopment() && Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
                builder.Configuration.AddJsonFile("appsettings.Docker-Development.json", false, true);
            if (builder.Environment.IsProduction())
                builder.Configuration.AddJsonFile("appsettings.Production.json", false, true);

            // Add environemnts vars
            builder.Configuration.AddEnvironmentVariables();

            builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

            return builder;
        }

        /// <summary>
        /// Handler for bad request responses
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureMVC(this WebApplicationBuilder builder)
        {
            // Add Mvc and custom options
            builder.Services.AddMvcCore()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        return ModelStateValidationErrorBuilder.BuildBadRequest(actionContext.ModelState);
                    };
                }).AddApiExplorer();

            return builder;
        }

        /// <summary>
        /// Setup the webserver for Linux and Windows server
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureWebServer(this WebApplicationBuilder builder)
        {
            // Ensure there is no limit for file upload size
            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = null;
                options.AllowSynchronousIO = true;
            });
            builder.Services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = null;
                options.AllowSynchronousIO = true;
                options.ConfigureHttpsDefaults(co =>
                {
                    co.SslProtocols = SslProtocols.Tls12;
                });
            });

            // For large file support
            builder.Services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });

            return builder;
        }

        /// <summary>
        /// Add rate limiting to the API
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureRateLimiting(this WebApplicationBuilder builder)
        {
            // For rate limiting
            builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimitSettings"));
            builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            // inject counter and rules stores
            builder.Services.AddInMemoryRateLimiting();

            return builder;
        }

        /// <summary>
        /// Configure Redis
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureCache(this WebApplicationBuilder builder)
        {
            /*builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
                options.InstanceName = "PricingCache";
            });*/

            builder.Services.AddTransient<IDistributedCache, InMemoryCache>();
            builder.Services.AddSingleton<ICacheService, CacheService>();

            return builder;
        }

        /// <summary>
        /// Configure service level http clients
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureHttpClients(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient();

            // Add HTTP clients
            builder.Services.AddHttpClient("TwelveDataClient", (client) => { client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("TwelveDataProvider:Endpoint")); });
            builder.Services.AddHttpClient("FreeCurrencyClient", (client) => { client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("FreeCurrencyService:Endpoint")); });
            builder.Services.AddHttpClient("MobulaClient", (client) => { client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("MobulaService:Endpoint")); });
            builder.Services.AddHttpClient("PolygonIOClient", (client) => { client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("PolygonIOService:Endpoint")); });
            builder.Services.AddHttpClient("MetalsDevClient", (client) => { client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("MetalsDevService:Endpoint")); });

            return builder;
        }

        /// <summary>
        /// Add Options from configuration to API
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureOptions(this WebApplicationBuilder builder)
        {
            // Add Options
            builder.Services.Configure<IdentityOptions>(options => { });
            builder.Services.Configure<TwelveDataServiceOptions>(options => builder.Configuration.GetSection("TwelveDataService").Bind(options));
            builder.Services.Configure<FreeCurrencyOptions>(options => builder.Configuration.GetSection("FreeCurrencyService").Bind(options));
            builder.Services.Configure<MobulaServiceOptions>(options => builder.Configuration.GetSection("MobulaService").Bind(options));
            builder.Services.Configure<PolygonIOServiceOptions>(options => builder.Configuration.GetSection("PolygonIOService").Bind(options));
            builder.Services.Configure<MetalsDevServiceOptions>(options => builder.Configuration.GetSection("MetalsDevService").Bind(options));

            builder.Services.Configure<JwtOptions>(options => builder.Configuration.GetSection("Jwt").Bind(options));
            builder.Services.Configure<JobOptions>(options => builder.Configuration.GetSection("Jobs").Bind(options));

            return builder;
        }

        /// <summary>
        /// Configure the services for the application (add to DI)
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {

            builder.Services.AddTransient<IAssetPriceHistoryFactory, AssetPriceHistoryFactory>();
            builder.Services.AddTransient<IAssetPriceFactory, AssetPriceFactory>();
            builder.Services.AddTransient<IAssetFactory, AssetFactory>();

            builder.Services.AddTransient<FreeCurrencyCurrencyPriceProvider>();
            builder.Services.AddTransient<TwelveDataCurrencyPriceProvider>();
            builder.Services.AddTransient<MobulaCurrencyPriceProvider>();
            builder.Services.AddTransient<PolygonIOCurrencyPriceProvider>();
            builder.Services.AddTransient<MetalsDevCurrencyPriceProvider>();

            //TODO: remove the delegate definition and move to resolver class
            builder.Services.AddTransient<AssetPriceServiceResolver>(serviceProvider => pricingProviderType =>
            {
                switch (pricingProviderType)
                {
                    case PricingProviderType.FreeCurrency:
                        return serviceProvider.GetService<FreeCurrencyCurrencyPriceProvider>();
                    case PricingProviderType.PolygonIO:
                        return serviceProvider.GetService<PolygonIOCurrencyPriceProvider>();
                    case PricingProviderType.Mobula:
                        return serviceProvider.GetService<MobulaCurrencyPriceProvider>();
                    case PricingProviderType.MetalsDev:
                        return serviceProvider.GetService<MetalsDevCurrencyPriceProvider>();
                    default:
                        throw new KeyNotFoundException($"Could not resolve service because type {pricingProviderType} not found");
                }
            });

            builder.Services.AddTransient<IAssetRepository, AssetRepository>();
            builder.Services.AddTransient<IAssetPriceRepository, AssetPriceRepository>();
            builder.Services.AddTransient<IAssetPriceHistoryRepository, AssetPriceHistoryRepository>();

            // Setup the data repositories
            builder.Services.AddTransient<ITwelveDataService, TwelveDataService>(s =>
                new TwelveDataService(s.GetService<IHttpClientFactory>().CreateClient("TwelveDataClient"),
                s.GetService<IOptions<TwelveDataServiceOptions>>(),
                s.GetService<ILogger<TwelveDataService>>())
            );

            builder.Services.AddTransient<IFreeCurrencyService, FreeCurrencyService>(s =>
                new FreeCurrencyService(s.GetService<IHttpClientFactory>().CreateClient("FreeCurrencyClient"),
                s.GetService<IOptions<FreeCurrencyOptions>>(),
                s.GetService<ILogger<FreeCurrencyService>>())
            );

            builder.Services.AddTransient<IMobulaService, MobulaService>(s =>
                new MobulaService(s.GetService<IHttpClientFactory>().CreateClient("MobulaClient"),
                s.GetService<IOptions<MobulaServiceOptions>>(),
                s.GetService<ILogger<MobulaService>>())
            );

            builder.Services.AddTransient<IPolygonIOService, PolygonIOService>(s =>
                new PolygonIOService(s.GetService<IHttpClientFactory>().CreateClient("PolygonIOClient"),
                s.GetService<IOptions<PolygonIOServiceOptions>>(),
                s.GetService<ILogger<PolygonIOService>>())
            );

            builder.Services.AddTransient<IMetalsDevService, MetalsDevService>(s =>
               new MetalsDevService(s.GetService<IHttpClientFactory>().CreateClient("MetalsDevClient"),
               s.GetService<IOptions<MetalsDevServiceOptions>>(),
               s.GetService<ILogger<MetalsDevService>>())
            );

            builder.Services.AddTransient<IJobsService, JobsService>();

            // Setup event handlers
            builder.Services.AddScoped<INotificationHandler<AssetPriceCreatedEvent>, AssetPriceCreatedEventHandler>();
            builder.Services.AddScoped<INotificationHandler<AssetPriceUpdatedEvent>, AssetPriceUpdatedEventHandler>();

            // Misc
            builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

            return builder;
        }

        /// <summary>
        /// Configure open tracing
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureOpenTracing(this WebApplicationBuilder builder)
        {
            builder.Services.AddOpenTracing();

            return builder;
        }

        /// <summary>
        /// Setup Mediatr
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureMediatr(this WebApplicationBuilder builder)
        {
            // Setup Mediatr service
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            builder.Services.AddScoped<IRequestHandler<GetPagedAssetsQuery, PagedResult<Asset>>, GetPagedAssetsQueryHandler>();
            builder.Services.AddScoped<IRequestHandler<GetAssetsQuery, List<Asset>>, GetAssetsQueryHandler>();
            builder.Services.AddScoped<IRequestHandler<CreateSupportedAssetCommand, Asset>, CreateSupportedAssetCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<UpdateSupportedAssetCommand, Asset>, UpdateSupportedAssetCommandHandler>();

            builder.Services.AddScoped<IRequestHandler<GetLatestPricesQuery, List<AssetPrice>>, GetLatestPricesQueryHandler>();
            builder.Services.AddScoped<IRequestHandler<CreateOrReplaceAssetPricesCommand, List<AssetPrice>>, CreateOrReplaceAssetPricesCommandHandler>();

            builder.Services.AddScoped<IRequestHandler<CreateAssetPriceHistoryCommand, AssetPriceHistory>, CreateAssetPriceHistoryCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<GetPagedAssetPriceHistoryQuery, PagedResult<AssetPriceHistory>>, GetPagedAssetPriceHistoryQueryHandler>();

            builder.Services.AddScoped<IRequestHandler<GetProviderPricesQuery, List<PricePair>>, GetProviderPricesQuerysHandler>();
            builder.Services.AddScoped<IRequestHandler<GetProviderAssetsQuery, List<ProviderAsset>>, GetProviderAssetsQuerysHandler>();
            builder.Services.AddScoped<IRequestHandler<ImportPricesCommand, bool>, ImportPricesCommandHandler>();

            return builder;
        }

        /// <summary>
        /// Configure the Hangfire delayed execution framework
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureHangfire(this WebApplicationBuilder builder)
        {
            // Set credentials
            var connectionStringWithCredentials = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("HangfireDatabase"));
            connectionStringWithCredentials.UserID = builder.Configuration.GetValue<string>("HangfireDatabaseUsername");
            connectionStringWithCredentials.Password = builder.Configuration.GetValue<string>("ApplicationDatabasePassword");

            // Hangfire queues
            builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionStringWithCredentials.ToString()));

            // Only uncomment if it is required to process events on this server (currently should be limited to events api)
            if (builder.Configuration.GetValue<bool>("Hangfire:ServerEnabled"))
            {
                builder.Services.AddHangfireServer(options =>
                {
                    options.WorkerCount = builder.Configuration.GetValue<int>("Hangfire:WorkerCount");
                });

                // Stop failed jobs from being rerun automatically. This enforces manual intervention in the point of failure
                GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
            }

            return builder;
        }

        /// <summary>
        /// Configure API level automapper for response mapping (add to DI)
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureAutomapper(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(MshrmStudioPricingMappingProfile).Assembly);

            return builder;
        }

        /// <summary>
        /// Add the db contexts (EF Core setup)
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureDbContexts(this WebApplicationBuilder builder)
        {
            var password = builder.Configuration.GetValue<string>("ApplicationDatabasePassword");

            builder.Services.SetContext<MshrmStudioPricingDbContext>(
                builder.Configuration.GetValue<string>("ApplicationDatabaseUsername"),
                password,
                builder.Configuration.GetConnectionString("ApplicationDatabase"),
                "Mshrm.Studio.Pricing.Infrastructure"
            );

            return builder;
        }

        /// <summary>
        /// Add background jobs to API
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureHostedServices(this WebApplicationBuilder builder)
        {
            /*
            builder.Services.AddHostedService<MobulaCronJob>();
            builder.Services.AddHostedService<FreeCurrencyCronJob>();
            builder.Services.AddHostedService<PolygonIOCronJob>();
            builder.Services.AddHostedService<MetalsDevCronJob>();
            */

            return builder;
        }

        /// <summary>
        /// Configure the authentication for the API
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            // Get JWT options
            var jwtOptions = new JwtOptions();
            builder.Configuration.GetSection("Jwt").Bind(jwtOptions);

            // Get OpenID Options
            var openIdOptions = new OpenIdOptions();
            builder.Configuration.GetSection("OpenId").Bind(openIdOptions);

            // For debugging
            IdentityModelEventSource.ShowPII = true; //builder.Environment.IsDevelopment();
            IdentityModelEventSource.LogCompleteSecurityArtifact = true; //builder.Environment.IsDevelopment();

            // Setup JWT Auth
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"{jwtOptions.Audience}"; // IdentityServer URL
                options.Audience = $"{jwtOptions.Audience}/resources"; // The audience for your API
                options.RequireHttpsMetadata = false; // Use true in production

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = $"{jwtOptions.Issuer}",
                    ValidAudiences = jwtOptions.ValidAudiences,
                    ValidIssuers = jwtOptions.ValidIssuers,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerValidator = (issuer, securityToken, validationParameters) => IssuerHelper.ValidateIssuer(issuer, securityToken, validationParameters),
                    IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
                    {
                        var securityKeys = new List<SecurityKey>();

                        var mshrmStudioConfigManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                           $"{jwtOptions.Audience}/.well-known/openid-configuration",
                            new OpenIdConnectConfigurationRetriever(),
                            new HttpDocumentRetriever { RequireHttps = false }
                        );

                        var mshrmStudioConfig = mshrmStudioConfigManager.GetConfigurationAsync().Result;
                        var mshrmStudioSigningKeys = mshrmStudioConfig.SigningKeys;

                        var microsoftConfigManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                         $"https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration",
                          new OpenIdConnectConfigurationRetriever(),
                          new HttpDocumentRetriever { RequireHttps = false }
                        );

                        var microsoftConfig = microsoftConfigManager.GetConfigurationAsync().Result;
                        var microsoftSigningKeys = microsoftConfig.SigningKeys;

                        securityKeys.AddRange(microsoftSigningKeys);
                        securityKeys.AddRange(mshrmStudioSigningKeys);

                        return securityKeys;
                    }
                };
            });

            // Setup Authorization for role base access
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "mshrm-studio-api");
                });
            });

            return builder;
        }

        /// <summary>
        /// Configure the API health checks
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureHealthChecks(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks();

            return builder;
        }

        /// <summary>
        /// Configure the API documentation
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureSwagger(this WebApplicationBuilder builder)
        {
            // Add Swagger docs
            builder.Services.AddSwaggerGen(c =>
            {
                var version = builder.Configuration.GetValue<string>("Swagger:Version");

                c.SwaggerDoc(version, new OpenApiInfo { Title = builder.Configuration.GetValue<string>("Swagger:Title"), Version = version });
                c.CustomSchemaIds(x => x.FullName);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return builder;
        }

        /// <summary>
        /// Add the controllers to the API (endpoints)
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureControllers(this WebApplicationBuilder builder)
        {
            // Setup controller
            builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.ClientErrorMapping[404].Link = "https://httpstatuses.com/404";
            })
            .AddNewtonsoftJson(opts =>
            {
                opts.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            }).AddDapr();

            return builder;
        }

        /// <summary>
        /// Add Dapr to the API
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureDapr(this WebApplicationBuilder builder)
        {
            builder.Services.AddDaprClient();

            return builder;
        }

        /// <summary>
        /// Add localization to the API
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureLocalization(this WebApplicationBuilder builder)
        {
            // Setup DI
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Add locales
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var localizationOptions = new MshrmStudioLocalizationOptions();
                builder.Configuration.GetSection("Localization").Bind(localizationOptions);
                var supportedCultures = localizationOptions.SupportedCultures.ToArray();
                options.SetDefaultCulture(localizationOptions.DefaultCulture)
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);
                options.ApplyCurrentCultureToResponseHeaders = true;

                // Custom provider (take from header NOT query string)
                options.RequestCultureProviders.Clear();
                options.RequestCultureProviders.Add(new MshrmStudioRequestCultureProvider(options));
            });

            return builder;
        }

        /// <summary>
        /// Add exception/error mappings
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureHellang(this WebApplicationBuilder builder)
        {
            builder.Services.AddProblemDetails(x =>
            {
                x.CustomizeProblemDetails = ctx =>
                {
                    var logger = ctx.HttpContext.RequestServices.GetService<ILogger<Program>>();

                    var exception = ctx.HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;
                    if (exception != null && exception is HttpActionValidationException)
                    {
                        var httpActionValidationException = exception as HttpActionValidationException;

                        ctx.ProblemDetails.Title = httpActionValidationException.StatusCode.ToString();
                        ctx.ProblemDetails.Status = (int)httpActionValidationException.StatusCode;
                        ctx.ProblemDetails.Detail = httpActionValidationException.FailedReason;
                        ctx.HttpContext.Response.StatusCode = (int)httpActionValidationException.StatusCode;
                        ctx.ProblemDetails.Extensions.Add("StackTrace", httpActionValidationException.StackTrace);
                        ctx.ProblemDetails.Extensions.Add("FailureCode", httpActionValidationException.FailureCode);
                    }
                    else if (exception != null)
                    {
                        logger.LogCritical("HERE2!");
                        ctx.ProblemDetails.Title = "Internal Server Error";
                        ctx.ProblemDetails.Detail = exception.Message;
                        ctx.ProblemDetails.Status = 500;
                        ctx.ProblemDetails.Extensions.Add("StackTrace", exception.StackTrace);
                        ctx.ProblemDetails.Extensions.Add("FailureCode", FailureCode.SystemError);
                    }
                };
            });

            return builder;
        }
    }
}
