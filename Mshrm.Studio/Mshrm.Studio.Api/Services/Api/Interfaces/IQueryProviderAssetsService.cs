using Mshrm.Studio.Api.Clients.Pricing;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface IQueryProviderAssetsService
    {
        /// <summary>
        /// Get all assets supported by a provider
        /// </summary>
        /// <param name="providerType">The pricing provider</param>
        /// <returns>The supported assets for a pricing provider</returns>
        Task<List<ProviderAssetDto>> GetProvidersAssetsAsync(PricingProviderType providerType);
    }
}
