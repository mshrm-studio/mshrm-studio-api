namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface IDeleteLocalizationService
    {
        /// <summary>
        /// Delete a localization resource
        /// </summary>
        /// <param name="guidId">The resources guid id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>An async task</returns>
        public Task DeleteLocalizationResourceAsync(Guid guidId, CancellationToken cancellationToken);
    }
}
