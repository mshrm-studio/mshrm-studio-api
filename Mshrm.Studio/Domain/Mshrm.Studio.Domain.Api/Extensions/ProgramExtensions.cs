using AspNetCoreRateLimit;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mshrm.Studio.Domain.Api.Context;
using Mshrm.Studio.Domain.Api.Mapping;
using Mshrm.Studio.Domain.Api.Repositories;
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
using Mshrm.Studio.Domain.Api.Models.Events;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Api.Repositories.Interfaces;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Repositories.Interfaces;
using Google.Api;
using Mshrm.Studio.Domain.Api.Models.Dtos.Tools;
using Mshrm.Studio.Domain.Api.Models.CQRS.Tools.Queries;
using Mshrm.Studio.Shared.Models.Dtos;
using Mshrm.Studio.Shared.Models.Pagination;
using Mshrm.Studio.Domain.Api.Models.CQRS.ContactForms.Commands;
using Mshrm.Studio.Domain.Api.Models.CQRS.ContactForms.Queries;
using Mshrm.Studio.Domain.Api.Models.CQRS.Users.Commands;
using Mshrm.Studio.Domain.Api.Models.CQRS.Users.Queries;
using OpenTracing;
using OpenTracing.Util;
using Mshrm.Studio.Domain.Api.Handlers.Event.Users;
using Mshrm.Studio.Domain.Api.Handlers.Request.Tools;
using Mshrm.Studio.Domain.Api.Handlers.Request.ContactForms;
using Mshrm.Studio.Domain.Api.Handlers.Request.Users;
using Mshrm.Studio.Domain.Domain.Users;
using Mshrm.Studio.Domain.Domain.Users.Events;
using Mshrm.Studio.Domain.Domain.Tools;
using System.Data.SqlClient;
using Mshrm.Studio.Domain.Infrastructure.Factories;
using Mshrm.Studio.Domain.Domain.ContactForms;
using Mshrm.Studio.Shared.Enums;

namespace Mshrm.Studio.Domain.Api.Extensions
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
        /// Configure service level http clients
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureHttpClients(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient();

            // Add HTTP clients
            //builder.Services.AddHttpClient("client", (client) => { client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("x:Endpoint")); });

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
            builder.Services.Configure<JwtOptions>(options => builder.Configuration.GetSection("Jwt").Bind(options));

            return builder;
        }

        /// <summary>
        /// Add the db contexts (EF Core setup)
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureDbContexts(this WebApplicationBuilder builder)
        {
            var username = builder.Configuration.GetValue<string>("ApplicationDatabaseUsername");
            var password = builder.Configuration.GetValue<string>("ApplicationDatabasePassword");

            builder.Services.SetContext<MshrmStudioDomainDbContext>(
                username,
                password,
                builder.Configuration.GetConnectionString("ApplicationDatabase"),
                "Mshrm.Studio.Domain.Infrastructure"
            );

            return builder;
        }

        /// <summary>
        /// Configure the services for the application (add to DI)
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IUserFactory, UserFactory>();
            builder.Services.AddTransient<IContactFormFactory, ContactFormFactory>();
            builder.Services.AddTransient<IToolFactory, ToolFactory>();

            // Setup the data repositories
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IContactFormRepository, ContactFormRepository>();
            builder.Services.AddTransient<IToolRepository, ToolRepository>();

            // Add event handlers
            builder.Services.AddScoped<INotificationHandler<UserCreatedEvent>, UserCreatedEventHandler>();

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

            builder.Services.AddScoped<IRequestHandler<CreateToolCommand, Tool>, CreateToolCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<UpdateToolCommand, Tool>, UpdateToolCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<GetToolsPagedQuery, PagedResult<Tool>>, GetToolsPagedQueryHandler>();
            builder.Services.AddScoped<IRequestHandler<GetToolByGuidQuery, Tool>, GetToolByGuidQueryHandler>();

            builder.Services.AddScoped<IRequestHandler<CreateContactFormCommand, ContactForm>, CreateContactFormCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<GetContactFormsPagedQuery, PagedResult<ContactForm>>, GetContactFormsPagedQueryHandler>();
            builder.Services.AddScoped<IRequestHandler<GetContactFormByGuidQuery, ContactForm>, GetContactFormByGuidQueryHandler>();

            builder.Services.AddScoped<IRequestHandler<CreateUserCommand, User>, CreateUserCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<GetUserByGuidQuery, User>, GetUserByGuidQueryHandler>();
            builder.Services.AddScoped<IRequestHandler<GetUserByIdQuery, User>, GetUserByIdQueryHandler>();
            builder.Services.AddScoped<IRequestHandler<GetUserByEmailQuery, User>, GetUserByEmailQueryHandler>();
            builder.Services.AddScoped<IRequestHandler<GetUsersPagedQuery, PagedResult<User>>, GetUsersPagedQueryHandler>();

            return builder;
        }

        /// <summary>
        /// Configure the Hanfire delayed execution framework
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureHangfire(this WebApplicationBuilder builder)
        {
            // Set credentials
            var connectionStringWithCredentials = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("HangfireDatabase"));
            connectionStringWithCredentials.UserID = builder.Configuration.GetValue<string>("HangfireDatabaseUsername");
            connectionStringWithCredentials.Password = builder.Configuration.GetValue<string>("HangfireDatabasePassword");

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
            builder.Services.AddAutoMapper(typeof(MshrmStudioDomainMappingProfile).Assembly);

            return builder;
        }

        /// <summary>
        /// Add background jobs to API
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureHostedServices(this WebApplicationBuilder builder)
        {
            // Hosted services
            if (builder.Configuration.GetValue<bool>("HostedServiceOptions:Enabled"))
            {
                //builder.Services.AddHostedService<SomeHostedService>();
            }

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

            // Get JWT signing keys
            var signingKeys = SigningKeyHelper.GetSigningKeysAsync(openIdOptions.WellKnownEndpoints).GetAwaiter().GetResult();

            // Setup JWT Auth
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // JWT Signing Key
                    IssuerSigningKey = SigningKeyHelper.CreateSigningKey(jwtOptions.JwtSigningKey),

                    // OIDC Signing Keys
                    IssuerSigningKeys = signingKeys,

                    // Name claim definition
                    NameClaimType = ClaimTypes.NameIdentifier,

                    // JWT Signing Key
                    ValidAudience = jwtOptions.Audience,

                    // External Audiences valid in JWT (to expect from)
                    ValidAudiences = jwtOptions.ValidAudiences,

                    // JWT Issuer
                    ValidIssuer = jwtOptions.Issuer,

                    // External Issuers valid in JWT (to expect from)
                    ValidIssuers = jwtOptions.ValidIssuers,

                    // Ensure issuer is validated
                    ValidateIssuer = true,

                    // Not required for ID4
                    ValidateAudience = false,

                    // Require check for expiration
                    RequireExpirationTime = true,

                    ValidateLifetime = true,

                    // So expriy works
                    ClockSkew = TimeSpan.Zero,

                    // Custom issuer validater to support multi tenant requests
                    IssuerValidator = (issuer, securityToken, validationParameters) => IssuerHelper.ValidateIssuer(issuer, securityToken, validationParameters),
                };

                // Add events for adding claims that OpenID cannot (ie. role)
                options.Events = JwtBearerEventHelper.CreateJwtBearerEvents();
            });

            // Setup Authorization for role base access
            //builder.Services.AddAuthorizationPolicies(builder.Configuration);

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
