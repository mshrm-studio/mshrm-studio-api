using AspNetCoreRateLimit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mshrm.Studio.Auth.Api.Context;
using Mshrm.Studio.Auth.Api.Mapping;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Shared.Builders;
using Mshrm.Studio.Shared.Helpers;
using Mshrm.Studio.Shared.Models.Options;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Providers;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Auth.Api.Models.Pocos;
using Mshrm.Studio.Auth.Domain.User.Commands;
using Mshrm.Studio.Auth.Domain.User.Queries;
using Mshrm.Studio.Auth.Domain.Tokens.Commands;
using Mshrm.Studio.Auth.Application.Services;
using Mshrm.Studio.Auth.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Mshrm.Studio.Auth.Domain.Users;
using Mshrm.Studio.Auth.Infrastructure.Factories;
using Mshrm.Studio.Shared.Enums;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Google.Api;
using Duende.IdentityServer.AspNetIdentity;
using IdentityModel;
using Microsoft.IdentityModel.Logging;
using Mshrm.Studio.Auth.Infrastructure.Repositories;
using Mshrm.Studio.Auth.Application.Handlers.Users;
using Mshrm.Studio.Auth.Domain.Clients;
using Mshrm.Studio.Auth.Domain.Clients.Commands;
using Mshrm.Studio.Auth.Application.Handlers.Clients;
using Mshrm.Studio.Auth.Domain.Clients.Queries;
using Mshrm.Studio.Shared.Models.Pagination;
using Mshrm.Studio.Auth.Domain.ApiResources;
using Mshrm.Studio.Auth.Domain.ApiResources.Commands;
using Mshrm.Studio.Auth.Application.Handlers.ApiResources;
using Mshrm.Studio.Auth.Domain.ApiResources.Queries;

namespace Mshrm.Studio.Auth.Api.Extensions
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
                .ConfigureApiBehaviorOptions(options => {}).AddApiExplorer();

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
            builder.Services.AddHttpClient("EmailApi", (client) => { client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("EmailApi:Url")); });

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

            // Set the contexts
            builder.Services.SetContext<MshrmStudioAuthDbContext>(
                username,
                password,
                builder.Configuration.GetConnectionString("ApplicationDatabase"),
                "Mshrm.Studio.Auth.Infrastructure"
            );

            builder.Services.SetContext<PersistedGrantDbContext>(
             username,
             password,
             builder.Configuration.GetConnectionString("ApplicationDatabase"),
             "Mshrm.Studio.Auth.Infrastructure"//typeof(ProgramExtensions).GetTypeInfo().Assembly.GetName().Name
           );

            builder.Services.SetContext<ConfigurationDbContext>(
              username,
              password,
              builder.Configuration.GetConnectionString("ApplicationDatabase"),
              "Mshrm.Studio.Auth.Infrastructure"//typeof(ProgramExtensions).GetTypeInfo().Assembly.GetName().Name
           );

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

            builder.Services.AddScoped<IRequestHandler<CreateUserCommand, MshrmStudioIdentityUser>, CreateUserCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<CreateUserAnyRoleCommand, MshrmStudioIdentityUser>, CreateUserAnyRoleCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<CreateUserFromSSOCommand, MshrmStudioIdentityUser>, CreateUserFromSSOCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<CreatePasswordResetTokenCommand>, CreatePasswordResetTokenCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<ResetPasswordCommand, bool>, ResetPasswordCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<UpdatePasswordCommand>, UpdatePasswordCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<CreateTokenCommand, Token>, CreateTokenCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<CreateRefreshTokenCommand, Token>, CreateRefreshTokenCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<ValidateUserConfirmationCommand, Token>, ValidateUserConfirmationCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<ResendUserConfirmationCommand, bool>, ResendUserConfirmationCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<GetUserByEmailQuery, MshrmStudioUser>, GetUserByEmailQueryHandler>();
            builder.Services.AddScoped<IRequestHandler<CreateClientCommand, ClientWithSecret>, CreateClientCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<GetPagedClientsQuery, PagedResult<Client>>, GetPagedClientsQueryHandler>();
            builder.Services.AddScoped<IRequestHandler<CreateApiResourceCommand, ApiResourceWithSecret>, CreateApiResourceCommandHandler>();
            builder.Services.AddScoped<IRequestHandler<GetPagedApiResourcesQuery, PagedResult<ApiResource>>, GetPagedApiResourcesQueryHandler>();

            return builder;
        }

        /// <summary>
        /// Configure the services for the application (add to DI)
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IMshrmStudioIdentityUserFactory, MshrmStudioIdentityUserFactory>();
            builder.Services.AddTransient<IClientFactory, ClientFactory>();
            builder.Services.AddTransient<IApiResourceFactory, ApiResourceFactory>();

            // Setup the managers
            builder.Services.AddTransient<UserManager<MshrmStudioIdentityUser>>();
            builder.Services.AddTransient<RoleManager<IdentityRole>>();

            // Setup the repositories
            builder.Services.AddTransient<IClientRepository, ClientRepository>();
            builder.Services.AddTransient<IApiResourceRepository, ApiResourceRepository>();

            // Setup the services
            builder.Services.AddTransient<IIdentityUserService, IdentityUserService>();

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
        /// Configure Identity for the API
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureIdentity(this WebApplicationBuilder builder)
        {
            // Setup Identity and roles
            builder.Services.AddIdentityCore<MshrmStudioIdentityUser>(opt =>
            {
                // The time span to be logged out for
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(builder.Configuration.GetValue<int>("Lockout:DefaultLockoutTimeSpanInMinutes"));
                // Attempts to lock out user
                opt.Lockout.MaxFailedAccessAttempts = builder.Configuration.GetValue<int>("Lockout:MaxFailedAccessAttempts");
                // Define what is accepted character wise in password
                opt.User.AllowedUserNameCharacters = builder.Configuration.GetValue<string>("Identity:AllowedPasswordCharacters");
            })
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<MshrmStudioIdentityUser, IdentityRole>>()
            .AddEntityFrameworkStores<MshrmStudioAuthDbContext>()
            .AddDefaultTokenProviders();

            // Get connection string to set up stores
            var username = builder.Configuration.GetValue<string>("ApplicationDatabaseUsername");
            var password = builder.Configuration.GetValue<string>("ApplicationDatabasePassword");
            var connectionStringWithCredentials = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("ApplicationDatabase"));
            connectionStringWithCredentials.UserID = username;
            connectionStringWithCredentials.Password = password;

            var migrationsAssembly = typeof(ProgramExtensions).GetTypeInfo().Assembly.GetName().Name;

            // Add identity server to API
            builder.Services.AddIdentityServer()
                .AddAspNetIdentity<MshrmStudioIdentityUser>()
                .AddDeveloperSigningCredential() //TODO: NOT something we want to use in a production environment
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator<MshrmStudioIdentityUser>>()
                .AddConfigurationStore(opt =>
                {
                    opt.ConfigureDbContext = c => c.UseSqlServer(connectionStringWithCredentials.ToString(), sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(opt =>
                {
                    opt.ConfigureDbContext = o => o.UseSqlServer(connectionStringWithCredentials.ToString(), sql => sql.MigrationsAssembly(migrationsAssembly));
                    opt.EnableTokenCleanup = true;
                });

            // Setup related services for identity user/role
            builder.Services.AddScoped<IUserValidator<MshrmStudioIdentityUser>, UserValidator<MshrmStudioIdentityUser>>();
            builder.Services.AddScoped<IPasswordValidator<MshrmStudioIdentityUser>, PasswordValidator<MshrmStudioIdentityUser>>();
            builder.Services.AddScoped<IPasswordHasher<MshrmStudioIdentityUser>, PasswordHasher<MshrmStudioIdentityUser>>();
            builder.Services.AddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            builder.Services.AddScoped<IRoleValidator<MshrmStudioIdentityUser>, RoleValidator<MshrmStudioIdentityUser>>();
            builder.Services.AddScoped<IdentityErrorDescriber>();
            builder.Services.AddScoped<ISecurityStampValidator, SecurityStampValidator<MshrmStudioIdentityUser>>();
            builder.Services.AddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<MshrmStudioIdentityUser>>();
            builder.Services.AddScoped<IUserClaimsPrincipalFactory<MshrmStudioIdentityUser>, UserClaimsPrincipalFactory<MshrmStudioIdentityUser, IdentityRole>>();
            builder.Services.AddScoped<UserManager<MshrmStudioIdentityUser>>();
            builder.Services.AddScoped<SignInManager<MshrmStudioIdentityUser>>();
            builder.Services.AddScoped<RoleManager<IdentityRole>>();

            // What to include in token
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserIdClaimType = JwtClaimTypes.Subject;
                options.ClaimsIdentity.UserNameClaimType = JwtClaimTypes.Name;
                options.ClaimsIdentity.RoleClaimType = JwtClaimTypes.Role;
            });

            // Identity password hash config
            builder.Services.Configure<PasswordHasherOptions>(options => options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3);

            // For debugging
            IdentityModelEventSource.ShowPII = builder.Environment.IsDevelopment();
            IdentityModelEventSource.LogCompleteSecurityArtifact = builder.Environment.IsDevelopment();

            return builder;
        }

        /// <summary>
        /// Configure API level automapper for response mapping (add to DI)
        /// </summary>
        /// <param name="builder">The api builder</param>
        /// <returns>The api builder</returns>
        public static WebApplicationBuilder ConfigureAutomapper(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(MshrmStudioAuthMappingProfile).Assembly);

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
                //builder.Services.AddHostedService<MonthlyRewardInstructionIssuerHostedService>();
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

                    // Ensure audience is validated
                    ValidateAudience = true,

                    // Require check for expiration
                    RequireExpirationTime = true,

                    ValidateLifetime = true,

                    // So expriy works
                    ClockSkew = TimeSpan.Zero,


                    // Custom issuer validater to support multi tenant requests
                    IssuerValidator = (issuer, securityToken, validationParameters) => IssuerHelper.ValidateIssuer(issuer, securityToken, validationParameters),

                    IssuerSigningKeyResolver = (string token, Microsoft.IdentityModel.Tokens.SecurityToken securityToken, string kid, Microsoft.IdentityModel.Tokens.TokenValidationParameters validationParameters) => new List<SecurityKey> { SigningKeyHelper.CreateSigningKey(jwtOptions.JwtSigningKey) }
                };

                // Add events for adding claims that OpenID cannot (ie. role)
                options.Events = JwtBearerEventHelper.CreateJwtBearerEvents();
            });

            // Setup Authorization for role base access
            builder.Services.AddAuthorizationPolicies(builder.Configuration);

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
                        ctx.ProblemDetails.Extensions.Add("StackTrace", exception.StackTrace);
                        ctx.ProblemDetails.Extensions.Add("FailureCode", FailureCode.SystemError);
                        ctx.ProblemDetails.Status = 500;
                    }
                };
            });

            return builder;
        }
    }
}
