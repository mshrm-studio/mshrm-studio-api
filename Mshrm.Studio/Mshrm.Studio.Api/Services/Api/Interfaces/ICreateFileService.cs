using Mshrm.Studio.Api.Clients.Storage;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface ICreateFileService
    {
        /// <summary>
        /// Upload a temporary file
        /// </summary>
        /// <param name="fileStream">The files stream</param>
        /// <param name="fileName">The name of the file</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The temporary upload information</returns>
        public Task<TemporaryFileUploadDto> UploadTemporaryFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken);
    }
}
