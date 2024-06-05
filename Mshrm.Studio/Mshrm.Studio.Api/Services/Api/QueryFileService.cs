using Mshrm.Studio.Api.Clients.Storage;
using Mshrm.Studio.Api.Services.Api.Interfaces;
using System;

namespace Mshrm.Studio.Api.Services.Api
{
    public class QueryFileService : IQueryFileService
    {
        private readonly IFileClient _fileClient;

        public QueryFileService(IFileClient fileClient)
        {
            _fileClient = fileClient;
        }

        /// <summary>
        /// Stream a file
        /// </summary>
        /// <param name="key">The key for the file</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A file stream</returns>
        public async Task<Stream> StreamFileAsync(Guid key, CancellationToken cancellationToken)
        {
            // Get file
            var file = await _fileClient.GetPublicFileAsync(key, null, cancellationToken);

            //var contentType = file.Headers["Content-Type"].FirstOrDefault();

            return file.Stream;
        }
    }
}
