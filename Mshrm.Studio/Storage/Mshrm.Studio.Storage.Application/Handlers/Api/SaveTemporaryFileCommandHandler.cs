using MediatR;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Helpers;
using Mshrm.Studio.Storage.Api.Models.CQRS.Files.Commands;
using Mshrm.Studio.Storage.Api.Models.Entities;
using Mshrm.Studio.Storage.Api.Repositories.Interfaces;
using Mshrm.Studio.Storage.Api.Services.Http.Interfaces;
using Mshrm.Studio.Storage.Domain.Files;
using Mshrm.Studio.Storage.Domain.Resources;
using OpenTracing;

namespace Mshrm.Studio.Storage.Api.Handlers.Api
{
    /// <summary>
    /// Create a resource
    /// </summary>
    public class SaveTemporaryFileCommandHandler : IRequestHandler<SaveTemporaryFileCommand, Resource>
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly ISpacesService _spacesService;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveTemporaryFileCommandHandler"/> class.
        /// </summary>
        /// <param name="resourceRepository"></param>
        /// <param name="spacesService"></param>
        /// <param name="tracer"></param>
        public SaveTemporaryFileCommandHandler(IResourceRepository resourceRepository, ISpacesService spacesService, ITracer tracer)
        {
            _resourceRepository = resourceRepository;
            _spacesService = spacesService;

            _tracer = tracer;
        }

        /// <summary>
        /// Save a temporary file
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The saved resource</returns>
        public async Task<Resource> Handle(SaveTemporaryFileCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("SaveTemporaryFileAsync_CreateResourceService").StartActive(true))
            {
                // Move from temp to perm
                var permKey = await _spacesService.MoveFileAsync(command.Key, "temp");

                // Check move was successful
                if (string.IsNullOrEmpty(permKey))
                    throw new NotFoundException("Temporary file not found", FailureCode.TemporaryFileNotFound);

                // Get file meta data
                var metaData = new MshrmStudioFile(permKey, command.FileName).GetFileMetaData();

                // Create resource
                return await _resourceRepository.CreateResourceAsync(permKey, metaData.FileName, metaData.Extension, metaData.AssetType, command.IsPrivate, cancellationToken);
            }
        }
    }
}
