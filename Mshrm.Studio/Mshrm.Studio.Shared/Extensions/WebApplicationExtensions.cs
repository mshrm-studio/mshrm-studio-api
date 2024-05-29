using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> AddDatabaseMigrationAsync<T>(this WebApplication webApplication) where T : DbContext
        {
            await using var scope = webApplication.Services.CreateAsyncScope();
            var serviceProvider = scope.ServiceProvider;

            var database = serviceProvider.GetService<T>();
            database.Database.Migrate();

            return webApplication;
        }
    }
}
