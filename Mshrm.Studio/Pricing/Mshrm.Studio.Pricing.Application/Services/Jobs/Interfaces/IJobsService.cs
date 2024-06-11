using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Services.Jobs.Interfaces
{
    public interface IJobsService
    {
        /// <summary>
        /// Pull in asset pair updates
        /// </summary>
        /// <param name="type">Provider type</param>
        /// <returns>An async task</returns>
        public Task<bool> ImportProviderPairsAsync(PricingProviderType type);
    }
}
