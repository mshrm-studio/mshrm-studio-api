using MediatR;
using Mshrm.Studio.Domain.Api.Models.CQRS.ContactForms.Queries;
using Mshrm.Studio.Domain.Api.Models.CQRS.Tools.Queries;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Models.Enums;
using Mshrm.Studio.Domain.Api.Repositories.Interfaces;
using Mshrm.Studio.Domain.Domain.Tools;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Models.Pagination;
using OpenTracing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Mshrm.Studio.Domain.Api.Handlers.Request.Tools
{
    /// <summary>
    /// Query tools
    /// </summary>
    public class GetToolsPagedQueryHandler : IRequestHandler<GetToolsPagedQuery, PagedResult<Tool>>
    {
        private readonly IToolRepository _toolRepository;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetToolByGuidQueryHandler"/> class.
        /// </summary>
        /// <param name="toolRepository"></param>
        /// <param name="tracer"></param>
        public GetToolsPagedQueryHandler(IToolRepository toolRepository, ITracer tracer)
        {
            _toolRepository = toolRepository;
            _tracer = tracer;
        }

        /// <summary>
        /// Get a page of tools
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A page of tools</returns>
        public async Task<PagedResult<Tool>> Handle(GetToolsPagedQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetToolsAsync_QueryToolsService").StartActive(true))
            {
                var tools = await _toolRepository.GetToolsPagedAsync(query.SearchTerm, query.Name, query.ToolType, new Page(query.PageNumber, query.PerPage),
                new SortOrder(query.OrderProperty, query.Order), cancellationToken);

                return tools;
            }
        }
    }
}
