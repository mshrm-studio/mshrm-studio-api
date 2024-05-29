using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Files;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface ICreateCurrenciesService
    {
        /// <summary>
        /// Create a new currency
        /// </summary>
        /// <param name="logo">The logo for the currency</param>
        /// <param name="name">The name</param>
        /// <param name="symbol">The symbol</param>
        /// <param name="symbolNative">Display symbol</param>
        /// <param name="description">A description</param>
        /// <param name="currencyType">The type</param>
        /// <param name="providerType">The provider to import currency price</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The new currency</returns>
        Task<CurrencyDto> CreateCurrencyAsync(TemporaryFileDto? logo, string name, string symbol, string symbolNative, string description, CurrencyType currencyType, 
            PricingProviderType providerType, CancellationToken cancellationToken);
    }
}
