using Duende.IdentityServer.EntityFramework.Entities;
using MediatR;
using Mshrm.Studio.Auth.Domain.ApiResources;
using Mshrm.Studio.Auth.Domain.ApiResources.Queries;
using Mshrm.Studio.Shared.Models.Pagination;
using OpenTracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Application.Handlers.ApiResources
{
    public class GetPagedApiResourcesQueryHandler : IRequestHandler<GetPagedApiResourcesQuery, PagedResult<ApiResource>>
    {
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly ITracer _tracer;

        public GetPagedApiResourcesQueryHandler(IApiResourceRepository apiResourceRepository, ITracer tracer)
        {
            _apiResourceRepository = apiResourceRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Get a page of api resources
        /// </summary>
        /// <param name="query">The query data</param>
        /// <returns>A page of api resources</returns>
        public async Task<PagedResult<ApiResource>> Handle(GetPagedApiResourcesQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetPagedApiResourcesQueryHandler").StartActive(true))
            {
                var apiResources = await _apiResourceRepository.GetApiResourcesPagedAsync(query.SearchTerm, query.Name,
                    new Page(query.PageNumber, query.PerPage), new SortOrder(query.OrderProperty, query.Order), cancellationToken);

                return apiResources;
            }
        }
    }
}
