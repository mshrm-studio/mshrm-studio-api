using Mshrm.Studio.Api.Clients.Pricing;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface ICreateJobService
    {
        /// <summary>
        /// Import prices for a provider
        /// </summary>
        /// <param name="pricingProviderType">The provider to import prices for</param>
        /// <returns>Job completion</returns>
        public Task<bool> ImportPricesAsync(PricingProviderType pricingProviderType);
    }
}
