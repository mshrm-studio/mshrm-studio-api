using Mshrm.Studio.Api.Clients.Localization;
using Mshrm.Studio.Api.Services.Api.Interfaces;

namespace Mshrm.Studio.Api.Services.Api
{
    public class CreateLocalizationService : ICreateLocalizationService
    {
        private readonly ILocalizationClient _localizationClient;

        public CreateLocalizationService(ILocalizationClient localizationClient)
        {
            _localizationClient = localizationClient;
        }

        /// <summary>
        /// Create a new localization resource
        /// </summary>
        /// <param name="area">The area to create resource for</param>
        /// <param name="culture">The culture for the resource</param>
        /// <param name="key">The resource text key to localize</param>
        /// <param name="value">The value to supply upon localization</param>
        /// <param name="comment">Any comment/s</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The new localization resource</returns>
        public async Task<LocalizationResourceDto> CreateLocalizationResourceAsync(LocalizationArea area, string culture, string key, string value, string? comment,
            CancellationToken cancellationToken)
        {
            return await _localizationClient.CreateLocalizationResourcesAsync(new CreateLocalizationResourceDto()
            {
                Culture = culture,
                Key = key,
                Value = value,
                Comment = comment
            }, cancellationToken);
        }
    }
}
