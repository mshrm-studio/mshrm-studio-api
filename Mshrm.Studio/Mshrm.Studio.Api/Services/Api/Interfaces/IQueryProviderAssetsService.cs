using Mshrm.Studio.Api.Clients.Pricing;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface IQueryProviderAssetsService
    {
        /// <summary>
        /// Get all symbols supported by a provider (list of symbols)
        /// </summary>
        /// <param name="providerType">The pricing provider</param>
        /// <returns>The supported symbols for a pricing provider</returns>
        Task<List<string>> GetProvidersAssetSymbolsAsync(PricingProviderType providerType);
    }
}
