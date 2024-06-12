using MediatR;
using Mshrm.Studio.Localization.Api.Models.Constants;
using Mshrm.Studio.Localization.Api.Models.CQRS.LocalizationResources.Queries;
using Mshrm.Studio.Localization.Api.Models.Entities;
using Mshrm.Studio.Localization.Api.Models.Enums;
using Mshrm.Studio.Localization.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Services.Interfaces;
using OpenTracing;
using System.Diagnostics;

namespace Mshrm.Studio.Localization.Api.Services.Api
{
    public class GetLocalizationResourcesQueryHandler : IRequestHandler<GetLocalizationResourcesQuery, List<LocalizationResource>>
    {
        private readonly ILocalizationRepository _localizationRepository;
        private readonly ICacheService _cacheService;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLocalizationResourceByGuidQueryHandler"/> class.
        /// </summary>
        /// <param name="localizationRepository"></param>
        /// <param name="tracer"></param>
        /// <param name="cacheService"></param>
        public GetLocalizationResourcesQueryHandler(ILocalizationRepository localizationRepository, ITracer tracer, ICacheService cacheService)
        {
            _localizationRepository = localizationRepository;

            _tracer = tracer;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Get localization resources - read only
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>Localization resources</returns>
        public async Task<List<LocalizationResource>> Handle(GetLocalizationResourcesQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetLocalizationResourcesAsync_QueryLocalizationService").StartActive(true))
            {
                var resources = await _cacheService.GetOrSetItemAsync<List<LocalizationResource>>(
                    $"{MshrmStudioLocalizationConstants.LocalizationResourcesKey}_{query.Area}_{query.Culture}_{query.Key}",
                    async () => await _localizationRepository.GetLocalizationResourcesReadOnlyAsync(query.Area, query.Culture, query.Key, cancellationToken),
                    cancellationToken,
                    3
                );

                return resources;
            }
        }
    }
}
