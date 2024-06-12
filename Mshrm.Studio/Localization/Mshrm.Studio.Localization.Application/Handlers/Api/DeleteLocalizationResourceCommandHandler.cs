using MediatR;
using Mshrm.Studio.Localization.Api.Models.Constants;
using Mshrm.Studio.Localization.Api.Models.CQRS.LocalizationResources.Commands;
using Mshrm.Studio.Localization.Api.Models.Entities;
using Mshrm.Studio.Localization.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Services.Interfaces;
using OpenTracing;

namespace Mshrm.Studio.Localization.Api.Services.Api
{
    public class DeleteLocalizationResourceCommandHandler : IRequestHandler<DeleteLocalizationResourceCommand, bool>
    {
        private readonly ILocalizationRepository _localizationResository;
        private readonly ICacheService _cacheService;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteLocalizationResourceCommandHandler"/> class.
        /// </summary>
        /// <param name="localizationRepository"></param>
        /// <param name="tracer"></param>
        public DeleteLocalizationResourceCommandHandler(ILocalizationRepository localizationRepository, ICacheService cacheService, ITracer tracer)
        {
            _localizationResository = localizationRepository;

            _cacheService = cacheService;
            _tracer = tracer;
        }

        /// <summary>
        /// Delete a localization resource
        /// </summary>
        /// <param name="query">The resources guid id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>An async task</returns>
        public async Task<bool> Handle(DeleteLocalizationResourceCommand query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("DeleteLocalizationResourceAsync_DeleteLocalizationService").StartActive(true))
            {
                var existing = await _localizationResository.GetLocalizationResourceReadOnlyAsync(query.GuidId, cancellationToken);
                if (existing == null)
                {
                    throw new NotFoundException("Localization resource doesn't exist", FailureCode.LocalizationResourceDoesntExist);
                }

                await _localizationResository.DeleteLocalizationResourceAsync(query.GuidId, cancellationToken);

                // Clear cache so this is now included
                await _cacheService.ClearItemsThatStartWithAsync(MshrmStudioLocalizationConstants.LocalizationResourcesKey);

                return true;
            }
        }
    }
}
