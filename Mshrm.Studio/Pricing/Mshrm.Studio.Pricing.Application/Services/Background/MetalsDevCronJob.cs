using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Services.Jobs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Application.Services.Background
{
    public class MetalsDevCronJob : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MetalsDevCronJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timeSpan = new TimeSpan(3,0,0);//3h

            // Every day at 8am local time
            using var timer = new PeriodicTimer(timeSpan);

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                // Setup cron jobs
                using (var serviceScope = _serviceScopeFactory.CreateScope())
                {
                    var services = serviceScope.ServiceProvider;
                    var jobService = services.GetRequiredService<IJobsService>();

                    await jobService.ImportProviderPairsAsync(PricingProviderType.MetalsDev);
                }
            }
        }
    }
}
