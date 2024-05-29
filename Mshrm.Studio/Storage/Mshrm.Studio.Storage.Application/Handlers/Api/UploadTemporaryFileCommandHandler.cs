using MediatR;
using Mshrm.Studio.Storage.Api.Models.CQRS.Files.Commands;
using Mshrm.Studio.Storage.Api.Models.CQRS.Resources.Queries;
using Mshrm.Studio.Storage.Api.Models.Dtos.Files;
using Mshrm.Studio.Storage.Api.Models.Misc;
using Mshrm.Studio.Storage.Api.Services.Http.Interfaces;
using OpenTracing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mshrm.Studio.Storage.Api.Handlers.Api
{
    /// <summary>
    /// Create a temporary file
    /// </summary>
    public class UploadTemporaryFileCommandHandler : IRequestHandler<UploadTemporaryFileCommand, TemporaryFileUpload>
    {
        private readonly ISpacesService _spacesService;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadTemporaryFileCommandHandler"/> class.
        /// </summary>
        /// <param name="spacesService"></param>
        /// <param name="tracer"></param>
        public UploadTemporaryFileCommandHandler(ISpacesService spacesService, ITracer tracer)
        {
            _spacesService = spacesService;

            _tracer = tracer;
        }

        /// <summary>
        /// Create a new temporary file
        /// </summary>
        /// <param name="command">The file to create</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The new file key</returns>
        public async Task<TemporaryFileUpload> Handle(UploadTemporaryFileCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("CreateTemporaryFileAsync_CreateTemporaryFileService").StartActive(true))
            {
                // Create new name
                var name = Guid.NewGuid().ToString();

                // Upload in temp bucket
                var key = await _spacesService.UploadFileAsync(command.Stream, name, "temp");

                return new TemporaryFileUpload()
                {
                    CreatedDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddDays(3), // TODO: make this configurable
                    Key = key
                };
            }
        }
    }
}
