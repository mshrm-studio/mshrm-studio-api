using Mshrm.Studio.Api.Clients.Localization;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface ICreateLocalizationService
    {
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
        public Task<LocalizationResourceDto> CreateLocalizationResourceAsync(LocalizationArea area, string culture, string key, string value, string? comment,
            CancellationToken cancellationToken);
    }
}
