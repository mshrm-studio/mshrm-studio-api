using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Clients.Storage;
using Mshrm.Studio.Api.Models.Dtos.Files;
using Mshrm.Studio.Api.Services.Api.Interfaces;

namespace Mshrm.Studio.Api.Services.Api
{
    public class UpdateCurrenciesService : IUpdateCurrenciesService
    {
        private readonly ICurrenciesClient _currenciesClient;
        private readonly IFileClient _fileClient;

        public UpdateCurrenciesService(ICurrenciesClient currenciesClient, IFileClient fileClient)
        {
            _currenciesClient = currenciesClient;
            _fileClient = fileClient;
        }

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
        public async Task<CurrencyDto> UpdateCurrencyAsync(Guid currencyGuidId, string name, string? description, string symbolNative, PricingProviderType providerType,
            CurrencyType currencyType, TemporaryFileDto? logo, CancellationToken cancellationToken)
        {
            // Create logo
            ResourceDto? persistedLogo = null;
            if (logo != null)
                persistedLogo = await _fileClient.SaveTemporaryFileAsync(logo.TemporaryKey, logo.FileName, false, cancellationToken);

            // Update
            return await _currenciesClient.UpdateSupportedCurrencyAsync(currencyGuidId, new UpdateSupportedCurrencyDto()
            { 
                CurrencyType = currencyType,
                Name = name,
                Description = description,
                SymbolNative = symbolNative,
                ProviderType = providerType,
                LogoGuidId = persistedLogo?.GuidId
            },cancellationToken);
        }
    }
}
