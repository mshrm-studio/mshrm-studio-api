using Mshrm.Studio.Localization.Api.Models.Entities;
using Mshrm.Studio.Localization.Api.Models.Enums;

namespace Mshrm.Studio.Localization.Api.Repositories.Interfaces
{
    public interface ILocalizationRepository
    {
        /// <summary>
        /// Create a new localization resource
        /// </summary>
        /// <param name="area">The area to create resource for</param>
        /// <param name="culture">The culture for the resource</param>
        /// <param name="name">The resource text to localize</param>
        /// <param name="value">The value to supply upon localization</param>
        /// <param name="comment">Any comment/s</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The new localization resource</returns>
        public Task<LocalizationResource> CreateLocalizationResourceAsync(LocalizationArea area, string culture, string name, string value, string? comment,
            CancellationToken cancellationToken);

        /// <summary>
        /// Delete a localization resource
        /// </summary>
        /// <param name="guidId">The resources guid id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>An async task</returns>
        public Task DeleteLocalizationResourceAsync(Guid guidId, CancellationToken cancellationToken);

        /// <summary>
        /// Get localization resources - read only
        /// </summary>
        /// <param name="area">The area to create resource for</param>
        /// <param name="culture">The culture for the resource</param>
        /// <param name="key">The resource text key to localize</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>Localization resources</returns>
        public Task<List<LocalizationResource>> GetLocalizationResourcesReadOnlyAsync(LocalizationArea? area, string? culture, string? key, CancellationToken cancellationToken);

        /// <summary>
        /// Get a localization resource
        /// </summary>
        /// <param name="guidId">The resources guid id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The localization resource if exists</returns>
        public Task<LocalizationResource?> GetLocalizationResourceReadOnlyAsync(Guid guidId, CancellationToken cancellationToken);
    }
}
