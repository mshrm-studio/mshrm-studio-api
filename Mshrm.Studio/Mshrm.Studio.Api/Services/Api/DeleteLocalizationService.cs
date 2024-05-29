using Mshrm.Studio.Api.Clients.Localization;
using Mshrm.Studio.Api.Services.Api.Interfaces;

namespace Mshrm.Studio.Api.Services.Api
{
    public class DeleteLocalizationService : IDeleteLocalizationService
    {
        private readonly ILocalizationClient _localizationClient;

        public DeleteLocalizationService(ILocalizationClient localizationClient)
        {
            _localizationClient = localizationClient;
        }

        /// <summary>
        /// Delete a localization resource
        /// </summary>
        /// <param name="guidId">The resources guid id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>An async task</returns>
        public async Task DeleteLocalizationResourceAsync(Guid guidId, CancellationToken cancellationToken)
        {
            await _localizationClient.DeleteLocalizationResourcesAsync(guidId, cancellationToken);
        }
    }
}
