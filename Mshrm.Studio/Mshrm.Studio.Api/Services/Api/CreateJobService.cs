using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Services.Api.Interfaces;

namespace Mshrm.Studio.Api.Services.Api
{
    public class CreateJobService : ICreateJobService
    {
        private readonly IJobClient _jobsClient;

        public CreateJobService(IJobClient jobsClient)
        {
            _jobsClient = jobsClient;
        }

        /// <summary>
        /// Import prices for a provider
        /// </summary>
        /// <param name="pricingProviderType">The provider to import prices for</param>
        /// <returns>Job completion</returns>
        public async Task<bool> ImportPricesAsync(PricingProviderType pricingProviderType)
        {
            return await _jobsClient.ImportPricesAsync(pricingProviderType);
        }
    }
}
