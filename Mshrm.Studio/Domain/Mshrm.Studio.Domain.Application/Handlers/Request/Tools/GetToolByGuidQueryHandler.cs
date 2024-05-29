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
    public class GetToolByGuidQueryHandler : IRequestHandler<GetToolByGuidQuery, Tool>
    {
        private readonly IToolRepository _toolRepository;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetToolByGuidQueryHandler"/> class.
        /// </summary>
        /// <param name="toolRepository"></param>
        /// <param name="tracer"></param>
        public GetToolByGuidQueryHandler(IToolRepository toolRepository, ITracer tracer)
        {
            _toolRepository = toolRepository;
            _tracer = tracer;
        }

        /// <summary>
        /// Get a tool
        /// </summary>
        /// <param name="query">The guid ID for the tool</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A tool if exists</returns>
        public async Task<Tool> Handle(GetToolByGuidQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetToolAsync_QueryToolsService").StartActive(true))
            {
                var tool = await _toolRepository.GetToolAsync(query.GuidId, cancellationToken);
                if (tool == null)
                    throw new NotFoundException("Tool doesn't exist", FailureCode.ToolDoesntExist);

                return tool;
            }
        }
    }
}
