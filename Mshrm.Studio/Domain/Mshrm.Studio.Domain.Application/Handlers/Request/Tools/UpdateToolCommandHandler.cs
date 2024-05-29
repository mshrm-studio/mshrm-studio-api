using MediatR;
using Mshrm.Studio.Domain.Api.Models.Dtos.Tools;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Models.Enums;
using Mshrm.Studio.Domain.Api.Repositories.Interfaces;
using Mshrm.Studio.Domain.Domain.Tools;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using OpenTracing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mshrm.Studio.Domain.Api.Handlers.Request.Tools
{
    /// <summary>
    /// For updating tools
    /// </summary>
    public class UpdateToolCommandHandler : IRequestHandler<UpdateToolCommand, Tool>
    {
        private readonly IToolRepository _toolRepository;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateToolCommandHandler"/> class.
        /// </summary>
        /// <param name="toolRepository"></param>
        /// <param name="tracer"></param>
        public UpdateToolCommandHandler(IToolRepository toolRepository, ITracer tracer)
        {
            _toolRepository = toolRepository;
            _tracer = tracer;
        }

        /// <summary>
        /// Update a tool
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The updated tool</returns>
        public async Task<Tool> Handle(UpdateToolCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("UpdateToolAsync_UpdateToolsService").StartActive(true))
            {
                // Check exists
                var existing = await _toolRepository.GetToolAsync(command.GuidId, cancellationToken);
                if (existing == null)
                {
                    throw new NotFoundException("Tool doesn't exist", FailureCode.ToolDoesntExist);
                }

                // Update
                return await _toolRepository.UpdateToolAsync(command.GuidId, command.Name, command.Description, command.Link, command.Rank, command.LogoGuidId, command.ToolType, cancellationToken);
            }
        }
    }
}
