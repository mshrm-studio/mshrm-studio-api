using Hangfire;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Pricing.Api.Models.Options;
using Mshrm.Studio.Pricing.Api.Services.Jobs.Interfaces;

namespace Mshrm.Studio.Pricing.Api.Extensions
{
    /// <summary>
    /// Hangfire Job Configuration
    /// </summary>
    public static class JobConfigurationExtensions
    {
        /// <summary>
        /// Configure cron jobs for the application using Hangfire
        /// </summary>
        /// <param name="application">The application to configure cron jobs for</param>
        /// <returns>The application, configured</returns>
        public static WebApplication ConfigureHangfireCronJobs(this WebApplication application)
        {
            // Setup cron jobs
            using (var serviceScope = application.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var jobService = services.GetRequiredService<IJobsService>();
                var jobOptions = services.GetRequiredService<IOptions<JobOptions>>()?.Value;

                // Remove if exists
                RecurringJob.RemoveIfExists("Free Currency Currency Pair Import");
                RecurringJob.RemoveIfExists("Mobula Currency Pair Import");
                RecurringJob.RemoveIfExists("PolygonIO Currency Pair Import");
                RecurringJob.RemoveIfExists("MetalsDev Currency Pair Import");

                // Free currency import job
                RecurringJob.AddOrUpdate("Free Currency Currency Pair Import",
                    () => jobService.ImportProviderPairsAsync(Models.Enums.PricingProviderType.FreeCurrency),
                    Cron.MinuteInterval(jobOptions.FreeCurrencyJobDelayInMinutes));

                // Mobula currency import job
                RecurringJob.AddOrUpdate("Mobula Currency Pair Import",
                    () => jobService.ImportProviderPairsAsync(Models.Enums.PricingProviderType.Mobula),
                    Cron.MinuteInterval(jobOptions.MobulaJobDelayInMinutes));

                // PolygonIO currency import job
                RecurringJob.AddOrUpdate("PolygonIO Currency Pair Import",
                    () => jobService.ImportProviderPairsAsync(Models.Enums.PricingProviderType.PolygonIO),
                    Cron.MinuteInterval(jobOptions.PolygonIOJobDelayInMinutes));
   
                // Metals Dev currency import job
                RecurringJob.AddOrUpdate("MetalsDev Currency Pair Import",
                   () => jobService.ImportProviderPairsAsync(Models.Enums.PricingProviderType.MetalsDev),
                   Cron.HourInterval(23)); // TODO: make this dynamic
            }

            return application;
        }
    }
}
