using MediatR;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Helpers;
using Mshrm.Studio.Storage.Api.Models.CQRS.Resources.Queries;
using Mshrm.Studio.Storage.Api.Models.Misc;
using Mshrm.Studio.Storage.Api.Repositories.Interfaces;
using Mshrm.Studio.Storage.Api.Services.Http.Interfaces;
using OpenTracing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Mshrm.Studio.Storage.Api.Handlers.Api
{
    /// <summary>
    /// Query resources
    /// </summary>
    public class GetResourceStreamQueryHandler : IRequestHandler<GetResourceStreamQuery, ResourceStream>
    {
        private readonly IResourceRepository _resourceRepository;
        private readonly ISpacesService _spacesService;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetResourceStreamQueryHandler"/> class.
        /// </summary>
        /// <param name="resourceRepository"></param>
        /// <param name="spacesService"></param>
        public GetResourceStreamQueryHandler(IResourceRepository resourceRepository, ISpacesService spacesService, ITracer tracer)
        {
            _resourceRepository = resourceRepository;
            _spacesService = spacesService;

            _tracer = tracer;
        }

        /// <summary>
        /// Get the file stream of a resource
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The resource stream</returns>
        public async Task<ResourceStream> Handle(GetResourceStreamQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetResourceStreamAsync_QueryResourceService").StartActive(true))
            {
                // Get the resource
                var resource = await _resourceRepository.GetResourceAsync(query.ResourceId, cancellationToken);
                if (resource == null)
                    throw new NotFoundException("Resource doesn't exist", FailureCode.ResourceDoesntExist);

                // Check if is a private resource (requires authentication)
                if (!query.IsPrivate && resource.IsPrivate)
                    throw new ForbidException("Cannot view this resource unauthenticated", FailureCode.ResourceIsPrivate);

                // Get the stream
                var stream = await _spacesService.GetFileAsync(resource.Key);
                if (stream == null)
                    throw new NotFoundException("File doesn't exist", FailureCode.ResourceDoesntExist);

                return new ResourceStream()
                {
                    ContentType = resource.ContentType,
                    FileName = resource.FileName,
                    Stream = stream
                };
            }
        }
    }
}
