using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Mshrm.Studio.Api.Clients.Localization;
using Mshrm.Studio.Api.Services.Api.Interfaces;

namespace Mshrm.Studio.Api.Services.Api
{
    public class QueryLocalizationService : IQueryLocalizationService
    {
        private readonly ILocalizationClient _localizationClient;

        public QueryLocalizationService(ILocalizationClient localizationClient)
        {
            _localizationClient = localizationClient;
        }

        /// <summary>
        /// Get localization resources - read only
        /// </summary>
        /// <param name="area">The area to create resource for</param>
        /// <param name="culture">The culture for the resource</param>
        /// <param name="name">The resource text to localize</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>Localization resources</returns>
        public async Task<List<LocalizationResourceDto>> GetLocalizationResourcesAsync(LocalizationArea? area, string? culture, string? name, CancellationToken cancellationToken)
        {
            var observableResources = await _localizationClient.GetLocalizationResourcesAsync(area, culture, name, cancellationToken);

            return observableResources.ToList();
        }

        /// <summary>
        /// Get a localization resource
        /// </summary>
        /// <param name="guidId">The resources guid id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The localization resource if exists</returns>
        public async Task<LocalizationResourceDto> GetLocalizationResourceAsync(Guid guidId, CancellationToken cancellationToken)
        {
            return await _localizationClient.GetLocalizationResourceAsync(guidId, cancellationToken);
        }

        /// <summary>
        /// Gets all supported localization cultures
        /// </summary>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>All supported localization cultures</returns>
        public async Task<List<string>> GetSupportedLocalizationCulturesAsync(CancellationToken cancellationToken)
        {
            return (await _localizationClient.GetSupportedLocalizationCulturesAsync(cancellationToken))?.ToList() ?? new List<string>();
        }

        /// <summary>
        /// Gets all keys for localization area
        /// </summary>
        /// <param name="localizationArea">Get all keys for a localization area</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>All keys that can be used for a localization area</returns>
        public async Task<List<string>> GetLocalizationAreaKeysAsync(LocalizationArea localizationArea, CancellationToken cancellationToken)
        {
            return (await _localizationClient.GetLocalizationAreaKeysAsync(localizationArea, cancellationToken))?.ToList() ?? new List<string>();
        }
    }
}
