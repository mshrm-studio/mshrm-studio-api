﻿namespace Mshrm.Studio.Api.Extensions
{
    using System;
    using System.Data.SqlClient;
    using System.Reflection;
    using System.Security.Authentication;
    using System.Security.Claims;
    using System.Text.Encodings.Web;
    using System.Text.Json.Serialization;
    using System.Text.Unicode;
    using AspNetCoreRateLimit;
    using Hangfire;
    using MediatR;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.DataAnnotations;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.IdentityModel.Protocols;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Mshrm.Studio.Api.Clients;
    using Mshrm.Studio.Api.Clients.Auth;
    using Mshrm.Studio.Api.Clients.Domain;
    using Mshrm.Studio.Api.Clients.Email;
    using Mshrm.Studio.Api.Clients.Localization;
    using Mshrm.Studio.Api.Clients.Pricing;
    using Mshrm.Studio.Api.Clients.Storage;
    using Mshrm.Studio.Api.Models;
    using Mshrm.Studio.Api.Models.Options;
    using Mshrm.Studio.Api.Models.Options.Configurations;
    using Mshrm.Studio.Api.Services.Api;
    using Mshrm.Studio.Api.Services.Api.Interfaces;
    using Mshrm.Studio.Api.Services.Localization;
    using Mshrm.Studio.Shared.Builders;
    using Mshrm.Studio.Shared.Enums;
    using Mshrm.Studio.Shared.Exceptions.HttpAction;
    using Mshrm.Studio.Shared.Extensions;
    using Mshrm.Studio.Shared.Helpers;
    using Mshrm.Studio.Shared.Models;
    using Mshrm.Studio.Shared.Models.Options;
    using Mshrm.Studio.Shared.Providers;
    using Newtonsoft.Json;

    /// <summary>
    /// Extensions for program.cs
    /// </summary>
    public static class ProgramExtensions
    {
        /// <summary>
        /// Add CORS to API (restrict front-end access).
        /// </summary>
        /// <param name="builder">The api builder.</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors();

            return builder;
        }

        /// <summary>
        /// Add application settings + environment variables - environment specific.
        /// </summary>
        /// <param name="builder">The api builder.</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureSettings(this WebApplicationBuilder builder)
        {
            // Base app settings is loaded
            builder.Configuration.AddJsonFile("appsettings.json", false, true);

            // Environment dependant settings
            if (builder.Environment.IsDevelopment() && Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") != "true")
            {
                builder.Configuration.AddJsonFile("appsettings.Development.json", false, true);
            }

            if (builder.Environment.IsDevelopment() && Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
            {
                builder.Configuration.AddJsonFile("appsettings.Docker-Development.json", false, true);
            }

            if (builder.Environment.IsProduction())
            {
                builder.Configuration.AddJsonFile("appsettings.Production.json", false, true);
            }

            // Add environemnts vars
            builder.Configuration.AddEnvironmentVariables();

            builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

            return builder;
        }

        /// <summary>
        /// Handler for bad request responses.
        /// </summary>
        /// <param name="builder">The api builder.</param>
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
            .AddApiExplorer()
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    return builder.Services.BuildServiceProvider().GetService<IStringLocalizer>();
                };
            });

            return builder;
        }

        /// <summary>
        /// Setup the webserver for linux and windows server.
        /// </summary>
        /// <param name="builder">The api builder.</param>
        /// <returns>The api builder.</returns>
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
                options.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(30);
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
        /// Add rate limiting to the API.
        /// </summary>
        /// <param name="builder">The api builder.</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureRateLimiting(this WebApplicationBuilder builder)
        {
            // For rate limiting
            builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimit"));
            builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            // inject counter and rules stores
            builder.Services.AddInMemoryRateLimiting();

            return builder;
        }

        /// <summary>
        /// Configure service level http clients.
        /// </summary>
        /// <param name="builder">The api builder.</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureHttpClients(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient();

            // Add HTTP clients
            builder.Services.AddHttpClient("DomainApi", (client) => { client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("DomainApi:Url")); });
            builder.Services.AddHttpClient("LoginApi", (client) => { client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("LoginApi:Url")); });
            builder.Services.AddHttpClient("EmailApi", (client) => { client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("EmailApi:Url")); });
            builder.Services.AddHttpClient("PriceApi", (client) => { client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("PriceApi:Url")); });
            builder.Services.AddHttpClient("FileApi", (client) => { client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("FileApi:Url")); });
            builder.Services.AddHttpClient("LocalizationApi", (client) => { client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("LocalizationApi:Url")); });

            return builder;
        }

        /// <summary>
        /// Add Options from configuration to API.
        /// </summary>
        /// <param name="builder">The api builder.</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureOptions(this WebApplicationBuilder builder)
        {
            // Add Options
            builder.Services.Configure<IdentityOptions>(options => { });
            builder.Services.Configure<DomainApiOptions>(options => builder.Configuration.GetSection("DomainApi").Bind(options));
            builder.Services.Configure<LoginApiOptions>(options => builder.Configuration.GetSection("LoginApi").Bind(options));
            builder.Services.Configure<EmailApiOptions>(options => builder.Configuration.GetSection("EmailApi").Bind(options));
            builder.Services.Configure<JwtOptions>(options => builder.Configuration.GetSection("Jwt").Bind(options));
            builder.Services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureModelBindingLocalization>();

            return builder;
        }

        /// <summary>
        /// Configure the services for the application (add to DI).
        /// </summary>
        /// <param name="builder">The api builder.</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            // Setup the data services
            builder.Services.AddTransient<IQueryUserService, QueryUserService>();
            builder.Services.AddTransient<IQueryContactFormService, QueryContactFormService>();
            builder.Services.AddTransient<ICreateContactFormService, CreateContactFormService>();
            builder.Services.AddTransient<IQueryFileService, QueryFileService>();
            builder.Services.AddTransient<ICreateFileService, CreateFileService>();
            builder.Services.AddTransient<IQueryToolsService, QueryToolsService>();
            builder.Services.AddTransient<ICreateToolsService, CreateToolsService>();
            builder.Services.AddTransient<IUpdateToolsService, UpdateToolsService>();
            builder.Services.AddTransient<ICreateAssetService, CreateAssetService>();
            builder.Services.AddTransient<IUpdateAssetsService, UpdateAssetsService>();
            builder.Services.AddTransient<IDeleteLocalizationService, DeleteLocalizationService>();
            builder.Services.AddTransient<IQueryLocalizationService, QueryLocalizationService>();
            builder.Services.AddTransient<ICreateLocalizationService, CreateLocalizationService>();
            builder.Services.AddTransient<IQueryAssetService, QueryAssetService>();
            builder.Services.AddTransient<IQueryPricesService, QueryPricesService>();
            builder.Services.AddTransient<IQueryPriceHistoryService, QueryPriceHistoryService>();
            builder.Services.AddTransient<IQueryProviderAssetsService, QueryProviderAssetsService>();
            builder.Services.AddTransient<ICreateJobService, CreateJobService>();

            // Setup the Http services
            builder.Services.AddTransient<IDomainUserClient, DomainUserClient>(s =>
                new DomainUserClient(builder.Configuration.GetValue<string>("DomainApi:Url"), s.GetService<IHttpClientFactory>().CreateClient("DomainApi"), s.GetService<IHttpContextAccessor>())
            );

            builder.Services.AddTransient<IDomainContactFormClient, DomainContactFormClient>(s =>
              new DomainContactFormClient(builder.Configuration.GetValue<string>("DomainApi:Url"), s.GetService<IHttpClientFactory>().CreateClient("DomainApi"), s.GetService<IHttpContextAccessor>())
            );

            builder.Services.AddTransient<IIdentityUserClient, IdentityUserClient>(s =>
                new IdentityUserClient(builder.Configuration.GetValue<string>("LoginApi:Url"), s.GetService<IHttpClientFactory>().CreateClient("LoginApi"), s.GetService<IHttpContextAccessor>())
            );

            builder.Services.AddTransient<IDomainToolsClient, DomainToolsClient>(s =>
                new DomainToolsClient(builder.Configuration.GetValue<string>("DomainApi:Url"), s.GetService<IHttpClientFactory>().CreateClient("DomainApi"), s.GetService<IHttpContextAccessor>())
            );

            builder.Services.AddTransient<IAuthClient, AuthClient>(s =>
                new AuthClient(builder.Configuration.GetValue<string>("LoginApi:Url"), s.GetService<IHttpClientFactory>().CreateClient("LoginApi"), s.GetService<IHttpContextAccessor>())
            );

            builder.Services.AddTransient<IEmailClient, EmailClient>(s =>
                new EmailClient(builder.Configuration.GetValue<string>("EmailApi:Url"), s.GetService<IHttpClientFactory>().CreateClient("EmailApi"), s.GetService<IHttpContextAccessor>())
            );

            builder.Services.AddTransient<IPricesClient, PricesClient>(s =>
                new PricesClient(builder.Configuration.GetValue<string>("PriceApi:Url"), s.GetService<IHttpClientFactory>().CreateClient("PriceApi"), s.GetService<IHttpContextAccessor>())
            );

            builder.Services.AddTransient<IJobClient, JobClient>(s =>
               new JobClient(builder.Configuration.GetValue<string>("PriceApi:Url"), s.GetService<IHttpClientFactory>().CreateClient("PriceApi"), s.GetService<IHttpContextAccessor>())
            );

            builder.Services.AddTransient<IAssetsClient, AssetsClient>(s =>
                new AssetsClient(builder.Configuration.GetValue<string>("CurrenciesApi:Url"), s.GetService<IHttpClientFactory>().CreateClient("PriceApi"), s.GetService<IHttpContextAccessor>())
            );

            builder.Services.AddTransient<IFileClient, FileClient>(s =>
                new FileClient(builder.Configuration.GetValue<string>("FileApi:Url"), s.GetService<IHttpClientFactory>().CreateClient("FileApi"), s.GetService<IHttpContextAccessor>())
            );

            builder.Services.AddTransient<ILocalizationClient, LocalizationClient>(s =>
                 new LocalizationClient(builder.Configuration.GetValue<string>("LocalizationApi:Url"), s.GetService<IHttpClientFactory>().CreateClient("LocalizationApi"), s.GetService<IHttpContextAccessor>())
            );

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
        /// Configure API level automapper for response mapping (add to DI).
        /// </summary>
        /// <param name="builder">The api builder.</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureAutomapper(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(Mshrm.Studio.Api.Mapping.MshrmStudioMappingProfile).Assembly);

            return builder;
        }

        /// <summary>
        /// Add background jobs to API.
        /// </summary>
        /// <param name="builder">The api builder.</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureHostedServices(this WebApplicationBuilder builder)
        {
            // Hosted services
            if (builder.Configuration.GetValue<bool>("HostedServiceOptions:Enabled"))
            {
                //builder.Services.AddHostedService<>();
            }

            return builder;
        }

        /// <summary>
        /// Configure the authentication for the API.
        /// </summary>
        /// <param name="builder">The api builder.</param>
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
                
                // Setup events 
                options.Events = new JwtBearerEvents
                {
                    // Add role to claims if not there already
                    OnTokenValidated = async ctx =>
                    {
                        // Get the auth service
                        var authService = (IIdentityUserClient)ctx.HttpContext.RequestServices.GetService(typeof(IIdentityUserClient));

                        // Get the callers email - throw error if not provided
                        var email = ctx.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                        if (string.IsNullOrEmpty(email))
                            throw new Exception("Email is empty when it is expected");

                        // If role alreayd added, stop here. 
                        var role = ctx.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
                        if (role != null)
                        {
                            return;
                        }
                        
                        var user = await authService.GetIdentityUserAsync(email);
                        if (user == null)
                        {
                            ctx.HttpContext.Response.StatusCode = 401;
                            return;
                        }

                        // Create the claims
                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Role, user.Role.ToString())
                        };

                        // Create a new claims identity
                        var appIdentity = new ClaimsIdentity(claims);

                        // Add the identity to the principle
                        ctx.Principal?.AddIdentity(appIdentity);

                        return;
                    }
                };

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
        /// Configure the API health checks.
        /// </summary>
        /// <param name="builder">The api builder.</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureHealthChecks(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks();

            return builder;
        }

        /// <summary>
        /// Configure the API documentation.
        /// </summary>
        /// <param name="builder">The api builder.</param>
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
        /// Add the controllers to the API (endpoints).
        /// </summary>
        /// <param name="builder">The api builder.</param>
        /// <returns>The api builder.</returns>
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
            .AddXmlDataContractSerializerFormatters()
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            return builder;
        }

        /// <summary>
        /// Add localization to the API.
        /// </summary>
        /// <param name="builder">The api builder.</param>
        /// <returns>The api builder.</returns>
        public static WebApplicationBuilder ConfigureLocalization(this WebApplicationBuilder builder)
        {
            // Setup DI
            builder.Services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
            builder.Services.AddScoped<IStringLocalizer, ApiResourceStringLocalizer>();

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
        /// Add exception/error mappings.
        /// </summary>
        /// <param name="builder">The api builder.</param>
        /// <returns>The api builder.</returns>
        public static WebApplicationBuilder ConfigureHellang(this WebApplicationBuilder builder)
        {
            //MshrmStudioProblemDetails
            builder.Services.AddProblemDetails(x =>
            {
                x.CustomizeProblemDetails = ctx =>
                {
                    var exception = ctx.HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;
                    var strLocalizer = ctx.HttpContext.RequestServices.GetService<IStringLocalizer>();

                    if (exception != null && exception is HttpActionValidationException)
                    {
                        var httpActionValidationException = exception as HttpActionValidationException;

                        if (httpActionValidationException != null)
                        {
                            CreateHttpValidationProblemDetails(httpActionValidationException, ctx, strLocalizer);
                        }
                        else
                        {
                            Create500ProblemDetails(exception, ctx, strLocalizer);
                        }
                    }
                    else if (exception != null && exception is LoginApiException)
                    {
                        var loginApiException = exception as LoginApiException;
                        var propegatedProblemDetail = JsonConvert.DeserializeObject<MshrmStudioProblemDetails>(loginApiException.Response);

                        if (propegatedProblemDetail != null)
                        {
                            CreateHttpClientProblemDetails(propegatedProblemDetail, ctx, strLocalizer, "Mshrm.Studio.Auth.Api");
                        }
                        else
                        {
                            Create500ProblemDetails(exception, ctx, strLocalizer);
                        }
                    }
                    else if (exception != null && exception is DomainApiException)
                    {
                        var domainApiException = exception as DomainApiException;
                        var propegatedProblemDetail = JsonConvert.DeserializeObject<MshrmStudioProblemDetails>(domainApiException.Response);

                        if (propegatedProblemDetail != null)
                        {
                            CreateHttpClientProblemDetails(propegatedProblemDetail, ctx, strLocalizer, "Mshrm.Studio.Domain.Api");
                        }
                        else
                        {
                            Create500ProblemDetails(exception, ctx, strLocalizer);
                        }
                    }
                    else if (exception != null && exception is EmailApiException)
                    {
                        var emailApiException = exception as EmailApiException;
                        var propegatedProblemDetail = JsonConvert.DeserializeObject<MshrmStudioProblemDetails>(emailApiException.Response);

                        if (propegatedProblemDetail != null)
                        {
                            CreateHttpClientProblemDetails(propegatedProblemDetail, ctx, strLocalizer, "Mshrm.Studio.Email.Api");
                        }
                        else
                        {
                            Create500ProblemDetails(exception, ctx, strLocalizer);
                        }
                    }
                    else if (exception != null && exception is PricingApiException)
                    {
                        var pricingApiException = exception as PricingApiException;
                        var propegatedProblemDetail = JsonConvert.DeserializeObject<MshrmStudioProblemDetails>(pricingApiException.Response);

                        if (propegatedProblemDetail != null)
                        {
                            CreateHttpClientProblemDetails(propegatedProblemDetail, ctx, strLocalizer, "Mshrm.Studio.Pricing.Api");
                        }
                        else
                        {
                            Create500ProblemDetails(exception, ctx, strLocalizer);
                        }
                    }
                    else if (exception != null && exception is StorageApiException)
                    {
                        var storageApiException = exception as StorageApiException;
                        var propegatedProblemDetail = JsonConvert.DeserializeObject<MshrmStudioProblemDetails>(storageApiException.Response);

                        if (propegatedProblemDetail != null)
                        {
                            CreateHttpClientProblemDetails(propegatedProblemDetail, ctx, strLocalizer, "Mshrm.Studio.Storage.Api");
                        }
                        else
                        {
                            Create500ProblemDetails(exception, ctx, strLocalizer);
                        }
                    }
                    else if (exception != null && exception is LocalizationApiException)
                    {
                        var localizationApiException = exception as LocalizationApiException;
                        var propegatedProblemDetail = JsonConvert.DeserializeObject<MshrmStudioProblemDetails>(localizationApiException.Response);

                        if (propegatedProblemDetail != null)
                        {
                            CreateHttpClientProblemDetails(propegatedProblemDetail, ctx, strLocalizer, "Mshrm.Studio.Localization.Api");
                        }
                        else
                        {
                            Create500ProblemDetails(exception, ctx, strLocalizer);
                        }
                    }
                    else if (exception != null)
                    {
                        Create500ProblemDetails(exception, ctx, strLocalizer);
                    }
                };
            });

            return builder;
        }

        #region Helpers

        private static ProblemDetailsContext CreateHttpClientProblemDetails(MshrmStudioProblemDetails propegatedProblemDetail, ProblemDetailsContext context, IStringLocalizer strLocalizer, string instance)
        {
            context.ProblemDetails.Instance = instance;
            context.ProblemDetails.Title = propegatedProblemDetail.Title;
            context.ProblemDetails.Status = propegatedProblemDetail.Status;
            context.ProblemDetails.Detail = strLocalizer[propegatedProblemDetail.FailureCode.ToString()]; //propegatedProblemDetail.Detail;
            context.ProblemDetails.Extensions.Add("FailureCode", propegatedProblemDetail.FailureCode);
            context.HttpContext.Response.StatusCode = propegatedProblemDetail.Status.Value;

            //if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
            //{
            context.ProblemDetails.Extensions.Add("StackTrace", propegatedProblemDetail.StackTrace);
            //}

            return context;
        }

        private static ProblemDetailsContext CreateHttpValidationProblemDetails(HttpActionValidationException httpActionValidationException, ProblemDetailsContext context, IStringLocalizer strLocalizer)
        {
            context.ProblemDetails.Instance = "Mshrm.Studio.Api";
            context.ProblemDetails.Title = httpActionValidationException.StatusCode.ToString();
            context.ProblemDetails.Status = (int)httpActionValidationException.StatusCode;
            context.ProblemDetails.Detail = strLocalizer[httpActionValidationException.FailureCode.ToString()]; // httpActionValidationException.FailedReason;
            context.HttpContext.Response.StatusCode = (int)httpActionValidationException.StatusCode;
            context.ProblemDetails.Extensions.Add("FailureCode", httpActionValidationException.FailureCode);

            //if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
            //{
            context.ProblemDetails.Extensions.Add("StackTrace", httpActionValidationException.StackTrace);
            //}

            return context;
        }

        private static ProblemDetailsContext Create500ProblemDetails(Exception exception, ProblemDetailsContext context, IStringLocalizer strLocalizer)
        {
            context.ProblemDetails.Instance = "Mshrm.Studio.Api";
            context.ProblemDetails.Title = "Internal Server Error";
            context.ProblemDetails.Detail = exception.Message;
            context.ProblemDetails.Status = 500;
            context.ProblemDetails.Extensions.Add("FailureCode", FailureCode.SystemError);

            //if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
            //{
            context.ProblemDetails.Extensions.Add("StackTrace", exception.StackTrace);
            //}

            return context;
        }

        #endregion
    }
}
