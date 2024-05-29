using MediatR;
using Mshrm.Studio.Localization.Api.Models.Constants;
using Mshrm.Studio.Localization.Api.Models.CQRS.LocalizationResources.Commands;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Mshrm.Studio.Localization.Api.Services.Api
{
    public class GetLocalizationResourceByGuidQueryHandler : IRequestHandler<GetLocalizationResourceByGuidQuery, LocalizationResource>
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
        public GetLocalizationResourceByGuidQueryHandler(ILocalizationRepository localizationRepository, ITracer tracer, ICacheService cacheService)
        {
            _localizationRepository = localizationRepository;

            _tracer = tracer;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Get a localization resource
        /// </summary>
        /// <param name="query">The resources guid id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The localization resource if exists</returns>
        public async Task<LocalizationResource> Handle(GetLocalizationResourceByGuidQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("CreateCurrencyAsync_CreateCurrencyService").StartActive(true))
            {
                var localizationResource = await _localizationRepository.GetLocalizationResourceReadOnlyAsync(query.GuidId, cancellationToken);
                if (localizationResource == null)
                {
                    throw new NotFoundException("Localization resource doesn't exist", FailureCode.LocalizationResourceDoesntExist);
                }

                return localizationResource;
            }
        }
    }
}
