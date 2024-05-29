using Dapr.Client;
using MediatR;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Auth.Application.Services.Interfaces;
using Mshrm.Studio.Auth.Domain.User.Commands;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Extensions;
using System.Reflection.Metadata.Ecma335;

namespace Mshrm.Studio.Auth.Api.Services.Api
{
    /// <summary>
    /// For confirming users
    /// </summary>
    public class ResendUserConfirmationCommandHandler : IRequestHandler<ResendUserConfirmationCommand, bool>
    {
        private readonly IIdentityUserService _identityUserService;
        private readonly DaprClient _daprClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResendUserConfirmationCommandHandler"/> class.
        /// </summary>
        /// <param name="identityUserService"></param>
        /// <param name="daprClient"></param>
        public ResendUserConfirmationCommandHandler(IIdentityUserService identityUserService, DaprClient daprClient)
        {
            _identityUserService = identityUserService;
            _daprClient = daprClient;
        }

        /// <summary>
        /// Resends confirmation token
        /// </summary>
        /// <param name="query">The users email</param>
        /// <param name="cancellationToken">If the request is aborted</param>
        /// <returns>If the confirmation code was resent</returns>
        public async Task<bool> Handle(ResendUserConfirmationCommand query, CancellationToken cancellationToken)
        {
            // Check email is valid
            var isValidEmail = query.Email.IsValidEmail();
            if (!isValidEmail)
            {
                throw new UnprocessableEntityException("Email is not valid", FailureCode.EmailIsInvalid);
            }

            // Check already exists
            var existingUser = await _identityUserService.FindByUserNameAsync(query.Email);
            if (existingUser == null)
            {
                throw new NotFoundException("User already doesn't exist", FailureCode.UserDoesntExist);
            }

            // Get confirmation token
            var confirmToken = await _identityUserService.GenerateAccountConfirmationTokenAsync(query.Email);
            if (string.IsNullOrEmpty(confirmToken))
            {
                throw new UnprocessableEntityException("Failed to generate confirmation token", FailureCode.FailedToGenerateConfirmationToken);
            }

            // Send email for user confirmation
            await _daprClient.PublishEventAsync("pubsub", "send-email", new
            {
                EmailType = EmailType.AccountConfirmationEmail,
                ToEmailAddress = existingUser.Email,
                Link = $"?token={confirmToken}"
            }, cancellationToken);

            return true;
        }

    }
}
