
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
    public class CreatePasswordResetTokenCommandHandler : IRequestHandler<CreatePasswordResetTokenCommand>
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
        public CreatePasswordResetTokenCommandHandler(IIdentityUserService identityUserService, DaprClient daprClient, ILogger<UpdatePasswordCommand> logger)
        {
            _daprClient = daprClient;
            _identityUserService = identityUserService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a token for resetting a users password
        /// </summary>
        /// <param name="command">The email to request token for</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An async task</returns>
        public async Task Handle(CreatePasswordResetTokenCommand command, CancellationToken cancellationToken)
        {
            // Get the token
            var token = await _identityUserService.RequestPasswordResetTokenAsync(command.Email);
            if (string.IsNullOrEmpty(token))
                throw new ForbidException("Password could not be reset", FailureCode.FailedToGenerateResetToken);

            // Send an email with the token for user
            await _daprClient.PublishEventAsync("pubsub", "send-email", new
            {
                ToEmailAddress = command.Email,
                EmailType = EmailType.PasswordReset,
                Link = $"?token={token}"
            }, cancellationToken);

            return;
        }
    }
}
