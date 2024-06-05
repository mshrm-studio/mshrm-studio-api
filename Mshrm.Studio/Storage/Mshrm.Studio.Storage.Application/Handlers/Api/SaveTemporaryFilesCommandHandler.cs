using Amazon.Runtime.Internal.Util;
using MediatR;
using Microsoft.Extensions.Logging;
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
    public class SaveTemporaryFilesCommandHandler : IRequestHandler<SaveTemporaryFilesCommand, List<Resource>>
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly ISpacesService _spacesService;
        private readonly ITracer _tracer;
        private readonly ILogger<SaveTemporaryFilesCommandHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveTemporaryFileCommandHandler"/> class.
        /// </summary>
        /// <param name="resourceRepository"></param>
        /// <param name="spacesService"></param>
        /// <param name="tracer"></param>
        public SaveTemporaryFilesCommandHandler(IResourceRepository resourceRepository, ISpacesService spacesService, ITracer tracer,
            ILogger<SaveTemporaryFilesCommandHandler> logger)
        {
            _resourceRepository = resourceRepository;
            _spacesService = spacesService;

            _tracer = tracer;
            _logger = logger;
        }

        /// <summary>
        /// Save a temporary file
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The saved resource</returns>
        public async Task<List<Resource>> Handle(SaveTemporaryFilesCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("SaveTemporaryFilesAsync_CreateResourcesService").StartActive(true))
            {
                // Validate all exist first
                foreach (var fileCommand in command.SaveTemporaryFilesCommands)
                {
                    try
                    {
                        var file = await _spacesService.GetFileAsync(fileCommand.Key, fileCommand.DirectoryType.ToString().ToLower());
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.StackTrace);

                        throw new NotFoundException("Temporary file not found", FailureCode.TemporaryFileNotFound);
                    }
                }

                // Add all files
                var allFilesAdded = new List<MshrmStudioFile>();
                foreach (var fileCommand in command.SaveTemporaryFilesCommands)
                {
                    // Move from temp to perm
                    var permKey = await _spacesService.MoveTemporaryFileAsync(fileCommand.Key, fileCommand.DirectoryType.ToString().ToLower());

                    // Check move was successful
                    if (string.IsNullOrEmpty(permKey))
                        throw new NotFoundException("Temporary file not found", FailureCode.TemporaryFileNotFound);

                    // Get file meta data
                    var file = new MshrmStudioFile(permKey, fileCommand.FileName);
                    var metaData = file.GetFileMetaData();

                    allFilesAdded.Add(file);
                }

                // Create resource
                return await _resourceRepository.CreateResourcesAsync(allFilesAdded, true, cancellationToken);
            }
        }
    }
}
