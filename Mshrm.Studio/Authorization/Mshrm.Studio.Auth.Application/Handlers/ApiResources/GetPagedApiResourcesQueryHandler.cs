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
    public class GetPagedApiResourcesQueryHandler : IRequestHandler<GetPagedApiScopesQuery, PagedResult<ApiScope>>
    {
        private readonly IApiScopeRepository _apiScopeRepository;
        private readonly ITracer _tracer;

        public GetPagedApiResourcesQueryHandler(IApiScopeRepository apiScopeRepository, ITracer tracer)
        {
            _apiScopeRepository = apiScopeRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Get a page of api scopes
        /// </summary>
        /// <param name="query">The query data</param>
        /// <returns>A page of api resources</returns>
        public async Task<PagedResult<ApiScope>> Handle(GetPagedApiScopesQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetPagedApiResourcesQueryHandler").StartActive(true))
            {
                var apiResources = await _apiScopeRepository.GetApiScopesPagedAsync(query.SearchTerm, query.Name,
                    new Page(query.PageNumber, query.PerPage), new SortOrder(query.OrderProperty, query.Order), cancellationToken);

                return apiResources;
            }
        }
    }
}
