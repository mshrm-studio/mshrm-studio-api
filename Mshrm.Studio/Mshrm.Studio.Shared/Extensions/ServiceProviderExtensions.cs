using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace Mshrm.Studio.Shared.Extensions
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Sets a database context into the service collection
        /// </summary>
        /// <typeparam name="T">The type of context (T:DbContext)</typeparam>
        /// <param name="services">The service provider</param>
        /// <param name="connectionString">The connection string of the connection</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection SetContext<T>(this IServiceCollection services, string? username, string? password, string? connectionString, string? migrationsAssembly)
           where T : DbContext
        {
            // Basic validation
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("No connection string provided");
            }

            // Set credentials
            var connectionStringWithCredentials = new SqlConnectionStringBuilder(connectionString);
            connectionStringWithCredentials.UserID = username;
            connectionStringWithCredentials.Password = password;

            // Add the context
            return services.AddDbContext<T>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.UseSqlServer(connectionStringWithCredentials.ToString(), options =>
                {
                    // Enable retry with max count of 10
                    options.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null
                    // Command timeout of 60s 
                    ).CommandTimeout(60);

                    if (!string.IsNullOrEmpty(migrationsAssembly))
                    {
                        options.MigrationsAssembly(migrationsAssembly);
                    }

                }).UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            }, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Sets NeoOptimas authorization policies (roles)
        /// </summary>
        /// <param name="services">The service provider</param>
        /// <param name="config">Platform configuration</param>
        /// <returns>The service collection with set policies</returns>
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services, IConfiguration config)
        {
            // Set up the policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("mshrm-studio-access",
                        authBuilder =>
                        {
                            // Ensure user is authenticated
                            authBuilder.RequireAuthenticatedUser();
                            // Use JWT Bearer scheme
                            authBuilder.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                        }
                    );
            });

            return services;
        }
    }
}
