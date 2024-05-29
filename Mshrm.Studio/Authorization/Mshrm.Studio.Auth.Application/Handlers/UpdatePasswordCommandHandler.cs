using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using Dapr.Client;
using System.Threading;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using MediatR;
using Mshrm.Studio.Auth.Domain.User.Commands;
using Mshrm.Studio.Auth.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Mshrm.Studio.Auth.Api.Handlers
{
    /// <summary>
    /// For updating a users password
    /// </summary>
    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand>
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
        public UpdatePasswordCommandHandler(IIdentityUserService identityUserService, DaprClient daprClient, ILogger<UpdatePasswordCommand> logger)
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
        public async Task Handle(UpdatePasswordCommand command, CancellationToken cancellationToken)
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

        /// <summary>
        /// Resets a users password using the reset token
        /// </summary>
        /// <param name="email">The email to request token for</param>
        /// <param name="token">There reset token</param>
        /// <param name="newPassword">The new password to reset to</param>
        /// <returns>The operation outcome</returns>
        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var report = await _identityUserService.ResetPasswordAsync(email, token, newPassword);
            if (!report.Success)
            {
                // Log the errors
                _logger.LogError(string.Join(',', report.Errors.Select(x => $"{x.Code}: {x.Description} {Environment.NewLine}")));

                // Forbid the reset
                throw new ForbidException("Password could not be reset", FailureCode.FailedToUpdatePassword);
            }

            return true;
        }

        /// <summary>
        /// Update a password
        /// </summary>
        /// <param name="email">The person updating their password</param>
        /// <param name="oldPassword">The old password</param>
        /// <param name="newPassword">The new password</param>
        /// <returns>An async task</returns>
        public async Task UpdatePasswordAsync(string email, string oldPassword, string newPassword)
        {
            var reset = await _identityUserService.UpdatePasswordAsync(email, oldPassword, newPassword);
            if (!reset)
                throw new ForbidException("Failed to update password", FailureCode.FailedToUpdatePassword);

            return;
        }
    }
}
