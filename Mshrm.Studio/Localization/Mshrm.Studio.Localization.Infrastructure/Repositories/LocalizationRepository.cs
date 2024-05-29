using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Localization.Api.Contexts;
using Mshrm.Studio.Localization.Api.Models.Entities;
using Mshrm.Studio.Localization.Api.Models.Enums;
using Mshrm.Studio.Localization.Api.Repositories.Interfaces;
using Mshrm.Studio.Localization.Domain.LocalizationResources;
using Mshrm.Studio.Shared.Repositories.Bases;

namespace Mshrm.Studio.Localization.Api.Repositories
{
    public class LocalizationRepository : BaseRepository<LocalizationResource, MshrmStudioLocalizationDbContext>, ILocalizationRepository
    {
        private readonly ILocalizationResourceFactory _localizationResourceFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public LocalizationRepository(MshrmStudioLocalizationDbContext context, ILocalizationResourceFactory localizationResourceFactory) : base(context)
        {
            _localizationResourceFactory = localizationResourceFactory;
        }

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
        public async Task<LocalizationResource> CreateLocalizationResourceAsync(LocalizationArea area, string culture, string name, string value, string? comment,
            CancellationToken cancellationToken)
        {
            var localResource = _localizationResourceFactory.CreateLocalizationResouce(area, culture, name, value, comment);

            Add(localResource);
            await SaveAsync(cancellationToken);

            return localResource;
        }

        /// <summary>
        /// Delete a localization resource
        /// </summary>
        /// <param name="guidId">The resources guid id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>An async task</returns>
        public async Task DeleteLocalizationResourceAsync(Guid guidId, CancellationToken cancellationToken)
        {
            var localResource = await GetAll().FirstOrDefaultAsync(x => x.GuidId == guidId, cancellationToken);
            if (localResource != null)
            {
                Remove(localResource);
                await SaveAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Get localization resources - read only
        /// </summary>
        /// <param name="area">The area to create resource for</param>
        /// <param name="culture">The culture for the resource</param>
        /// <param name="name">The resource text to localize</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>Localization resources</returns>
        public async Task<List<LocalizationResource>> GetLocalizationResourcesReadOnlyAsync(LocalizationArea? area, string? culture, string? name, CancellationToken cancellationToken)
        {
            var localizationResources = GetAll()
                .AsNoTracking()
                .AsQueryable();

            if (area.HasValue)
            {
                localizationResources = localizationResources.Where(x => x.LocalizationArea == area);
            }

            if (!string.IsNullOrEmpty(culture))
            {
                localizationResources = localizationResources.Where(x => x.Culture == culture);
            }

            if (!string.IsNullOrEmpty(name))
            {
                localizationResources = localizationResources.Where(x => x.Name == name);
            }

            return await localizationResources.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Get a localization resource
        /// </summary>
        /// <param name="guidId">The resources guid id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The localization resource if exists</returns>
        public async Task<LocalizationResource?> GetLocalizationResourceReadOnlyAsync(Guid guidId, CancellationToken cancellationToken)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.GuidId == guidId, cancellationToken);
        }
    }
}
