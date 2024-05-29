using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Services.Jobs.Interfaces
{
    public interface IJobsService
    {
        /// <summary>
        /// Pull in currency pair updates
        /// </summary>
        /// <param name="type">Currency type</param>
        /// <returns>An async task</returns>
        public Task ImportProviderPairsAsync(PricingProviderType type);
    }
}
