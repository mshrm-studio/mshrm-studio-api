using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Mshrm.Studio.Localization.Api.Models.Enums;
using Mshrm.Studio.Localization.Application.Helpers;
using Mshrm.Studio.Localization.Domain.LocalizationResources.Queries;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using OpenTracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Localization.Application.Handlers.Api
{
    public class GetKeysForLocalizationAreaQueryHandler : IRequestHandler<GetKeysForLocalizationAreaQuery, List<string>>
    {
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetKeysForLocalizationAreaQueryHandler"/> class.
        /// </summary>
        /// <param name="tracer"></param>
        public GetKeysForLocalizationAreaQueryHandler(ITracer tracer)
        {
            _tracer = tracer;
        }

        /// <summary>
        /// Get all keys for a localization area
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>All keys for a localization area</returns>
        public async Task<List<string>> Handle(GetKeysForLocalizationAreaQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetKeysForLocalizationAreaQuery").StartActive(true))
            {
                return LocalizationAreaKeyHelper.GetKeys(query.LocalizationArea);
            }
        }
    }
}
