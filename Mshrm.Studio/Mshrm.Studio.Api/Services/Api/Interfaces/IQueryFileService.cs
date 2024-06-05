namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface IQueryFileService
    {
        /// <summary>
        /// Stream a file
        /// </summary>
        /// <param name="key">The key for the file</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A file stream</returns>
        public Task<Stream> StreamFileAsync(Guid key, CancellationToken cancellationToken);
    }
}
