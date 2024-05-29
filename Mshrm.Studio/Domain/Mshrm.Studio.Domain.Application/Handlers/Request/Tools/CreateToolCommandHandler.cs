using MediatR;
using Mshrm.Studio.Domain.Api.Models.CQRS.ContactForms.Commands;
using Mshrm.Studio.Domain.Api.Models.Dtos.Tools;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Models.Enums;
using Mshrm.Studio.Domain.Api.Repositories.Interfaces;
using Mshrm.Studio.Domain.Domain.Tools;
using OpenTracing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mshrm.Studio.Domain.Api.Handlers.Request.Tools
{
    public class CreateToolCommandHandler : IRequestHandler<CreateToolCommand, Tool>
    {
        private readonly IToolRepository _toolRepository;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateToolCommandHandler"/> class.
        /// </summary>
        /// <param name="toolRepository"></param>
        /// <param name="tracer"></param>
        public CreateToolCommandHandler(IToolRepository toolRepository, ITracer tracer)
        {
            _toolRepository = toolRepository;
            _tracer = tracer;
        }

        /// <summary>
        /// Create a new tool
        /// </summary>
        /// <param name="command">The create command</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The tool</returns>
        public async Task<Tool> Handle(CreateToolCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("CreateToolAsync_CreateToolsService").StartActive(true))
            {
                return await _toolRepository.CreateToolAsync(command.Name, command.Description, command.Link, command.Rank, command.LogoGuidId, command.ToolType, cancellationToken);
            }
        }
    }
}
