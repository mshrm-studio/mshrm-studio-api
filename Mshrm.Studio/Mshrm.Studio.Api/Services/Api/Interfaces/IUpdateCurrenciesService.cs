using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Files;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface IUpdateCurrenciesService
    {
        /// <summary>
        /// Update a currency
        /// </summary>
        /// <param name="currencyGuidId">The currency to update</param>
        /// <param name="name">The new name</param>
        /// <param name="description">A new description</param>
        /// <param name="symbolNative">The new display symbol</param>
        /// <param name="providerType">The new provider to import prices from for this currency</param>
        /// <param name="currencyType">The new type</param>
        /// <param name="logo">A new logo - not updated if left null</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The updated currency</returns>
        Task<CurrencyDto> UpdateCurrencyAsync(Guid currencyGuidId, string name, string? description, string symbolNative, PricingProviderType providerType, 
            CurrencyType currencyType, TemporaryFileDto? logo, CancellationToken cancellationToken);
    }
}
