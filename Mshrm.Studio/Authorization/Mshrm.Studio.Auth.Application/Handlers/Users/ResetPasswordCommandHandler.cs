using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using Dapr.Client;
using System.Threading;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using MediatR;
using Mshrm.Studio.Auth.Domain.User.Commands;
using Mshrm.Studio.Auth.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Mshrm.Studio.Auth.Application.Handlers.Users
{
    /// <summary>
    /// For updating a users password
    /// </summary>
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IIdentityUserService _identityUserService;
        private readonly DaprClient _daprClient;

        private readonly ILogger<UpdatePasswordCommand> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePasswordCommand"/> class.
        /// </summary>
        /// <param name="identityUserService"></param>
        /// <param name="daprClient"></param>
        /// <param name="logger"></param>
        public ResetPasswordCommandHandler(IIdentityUserService identityUserService, DaprClient daprClient, ILogger<UpdatePasswordCommand> logger)
        {
            _daprClient = daprClient;
            _identityUserService = identityUserService;
            _logger = logger;
        }

        /// <summary>
        /// Resets a users password using the reset token
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The operation outcome</returns>
        public async Task<bool> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var report = await _identityUserService.ResetPasswordAsync(command.Email, command.Token, command.NewPassword);
            if (!report.Success)
            {
                // Log the errors
                _logger.LogError(string.Join(',', report.Errors.Select(x => $"{x.Code}: {x.Description} {Environment.NewLine}")));

                // Forbid the reset
                throw new ForbidException("Password could not be reset", FailureCode.FailedToUpdatePassword);
            }

            return true;
        }
    }
}
