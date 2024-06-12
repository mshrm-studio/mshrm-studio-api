using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Mshrm.Studio.Localization.Api.Models.CQRS.LocalizationResources.Queries;
using Mshrm.Studio.Localization.Api.Models.Entities;
using Mshrm.Studio.Localization.Domain.LocalizationResources.Queries;
using OpenTracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Localization.Application.Handlers.Api
{
    public class GetSupportedLocalizationResourceCulturesQueryHandler : IRequestHandler<GetSupportedLocalizationResourceCulturesQuery, List<string>>
    {
        private readonly RequestLocalizationOptions _requestLocalizationOptions;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetKeysForLocalizationAreaQueryHandler"/> class.
        /// </summary>
        /// <param name="tracer"></param>
        public GetSupportedLocalizationResourceCulturesQueryHandler(IOptions<RequestLocalizationOptions> requestLocalizationOptions, ITracer tracer)
        {
            _requestLocalizationOptions = requestLocalizationOptions.Value;

            _tracer = tracer;
        }

        /// <summary>
        /// Get all keys for a localization area
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>All keys for a localization area</returns>
        public async Task<List<string>> Handle(GetSupportedLocalizationResourceCulturesQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetSupportedLocalizationResourceCulturesQuery").StartActive(true))
            {
                return _requestLocalizationOptions.SupportedCultures.Select(x => x.Name).ToList();
            }
        }
    }
}
