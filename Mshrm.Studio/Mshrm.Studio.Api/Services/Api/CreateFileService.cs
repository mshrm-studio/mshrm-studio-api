using Azure.Core;
using Mshrm.Studio.Api.Clients.Storage;
using Mshrm.Studio.Api.Services.Api.Interfaces;
using System.IO;

namespace Mshrm.Studio.Api.Services.Api
{
    public class CreateFileService : ICreateFileService
    {
        private readonly IFileClient _fileClient;

        public CreateFileService(IFileClient fileClient)
        {
            _fileClient = fileClient;
        }

        /// <summary>
        /// Upload a temporary file
        /// </summary>
        /// <param name="fileStream">The files stream</param>
        /// <param name="fileName">The name of the file</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The temporary upload information</returns>
        public async Task<TemporaryFileUploadDto> UploadTemporaryFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken)
        {
            return await _fileClient.UploadTemporaryFileAsync(new FileParameter(fileStream, fileName), cancellationToken);
        }
    }
}
