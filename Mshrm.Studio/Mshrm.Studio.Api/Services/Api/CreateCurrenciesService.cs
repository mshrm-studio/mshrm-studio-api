using Azure.Core;
using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Clients.Storage;
using Mshrm.Studio.Api.Models.Dtos.Files;
using Mshrm.Studio.Api.Services.Api.Interfaces;

namespace Mshrm.Studio.Api.Services.Api
{
    public class CreateCurrenciesService : ICreateCurrenciesService
    {
        private readonly ICurrenciesClient _currenciesClient;
        private readonly IFileClient _fileClient;

        public CreateCurrenciesService(ICurrenciesClient currenciesClient, IFileClient fileClient)
        {
            _currenciesClient = currenciesClient;
            _fileClient = fileClient;
        }

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
        public async Task<CurrencyDto> CreateCurrencyAsync(TemporaryFileDto? logo, string name, string symbol, string symbolNative, string description, CurrencyType currencyType,
            PricingProviderType providerType, CancellationToken cancellationToken)
        {
            // Create logo
            ResourceDto? persistedLogo = null;
            if (logo != null)
                persistedLogo = await _fileClient.SaveTemporaryFileAsync(logo.TemporaryKey, logo.FileName, false, cancellationToken) ;

            // Create currency
            return await _currenciesClient.CreateSupportedCurrencyAsync(new CreateSupportedCurrencyDto()
            {
                CurrencyType = currencyType,
                Description = description,
                Symbol = symbol,
                SymbolNative = symbolNative,
                Name = name,
                ProviderType = providerType,
                LogoGuidId = persistedLogo?.GuidId
            }, cancellationToken);
        }
    }
}
