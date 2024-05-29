using MediatR;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Storage.Api.Models.CQRS.Resources.Queries;
using Mshrm.Studio.Storage.Api.Models.Dtos.Files;
using Mshrm.Studio.Storage.Api.Models.Enums;
using Mshrm.Studio.Storage.Api.Models.Misc;
using Mshrm.Studio.Storage.Api.Services.Http.Interfaces;
using OpenTracing;

namespace Mshrm.Studio.Storage.Api.Handlers.Api
{
    /*public class GetFileQueryHandler : IRequestHandler<, Stream?>
    {
        private readonly ISpacesService _spacesService;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryFileService"/> class.
        /// </summary>
        /// <param name="spacesService"></param>
        /// <param name="tracer"></param>
        public GetFileQueryHandler(ISpacesService spacesService, ITracer tracer)
        {
            _spacesService = spacesService;

            _tracer = tracer;
        }

        /// <summary>
        /// Get a file
        /// </summary>
        /// <param name="key">The files key</param>
        /// <returns>The sub location in bucket</returns>
        public async Task<Stream?> GetFileAsync(string key, FileType type)
        {
            using (var scope = _tracer.BuildSpan("GetFileAsync_QueryFileService").StartActive(true))
            {
                // Check we have something for the key
                if (string.IsNullOrEmpty(key))
                    throw new UnprocessableEntityException("Key must be provided", FailureCode.KeyNotProvided);

                // Set sub location
                string? filePath = null;
                switch (type)
                {
                    case FileType.Temporary: filePath = "temp"; break;
                    case FileType.Permanent:
                    default: break;
                }

                // Get file
                var file = await _spacesService.GetFileAsync(key, filePath);
                if (file == null)
                    throw new NotFoundException("File not found", FailureCode.FileNotFound);

                return file;
            }
        }
    }*/
}
