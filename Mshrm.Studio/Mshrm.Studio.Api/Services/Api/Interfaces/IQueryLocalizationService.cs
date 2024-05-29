using Mshrm.Studio.Api.Clients.Localization;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface IQueryLocalizationService
    {
        /// <summary>
        /// Get localization resources - read only
        /// </summary>
        /// <param name="area">The area to create resource for</param>
        /// <param name="culture">The culture for the resource</param>
        /// <param name="name">The resource text to localize</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>Localization resources</returns>
        public Task<List<LocalizationResourceDto>> GetLocalizationResourcesAsync(LocalizationArea? area, string? culture, string? name, CancellationToken cancellationToken);

        /// <summary>
        /// Get a localization resource
        /// </summary>
        /// <param name="guidId">The resources guid id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The localization resource if exists</returns>
        public Task<LocalizationResourceDto> GetLocalizationResourceAsync(Guid guidId, CancellationToken cancellationToken);
    }
}
