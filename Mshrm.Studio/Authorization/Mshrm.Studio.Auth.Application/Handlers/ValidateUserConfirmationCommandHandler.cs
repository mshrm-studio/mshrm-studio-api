using Dapr.Client;
using MediatR;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Auth.Api.Services.Api;
using Mshrm.Studio.Auth.Application.Services.Interfaces;
using Mshrm.Studio.Auth.Domain.User.Commands;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Extensions;
using System.Reflection.Metadata.Ecma335;

namespace Mshrm.Studio.Auth.Api.Handlers
{
    /// <summary>
    /// For confirming users
    /// </summary>
    public class ValidateUserConfirmationCommandHandler : IRequestHandler<ValidateUserConfirmationCommand, Token>
    {
        private readonly IIdentityUserService _identityUserService;
        private readonly DaprClient _daprClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateUserConfirmationCommand"/> class.
        /// </summary>
        /// <param name="identityUserService"></param>
        /// <param name="daprClient"></param>
        public ValidateUserConfirmationCommandHandler(IIdentityUserService identityUserService, DaprClient daprClient)
        {
            _identityUserService = identityUserService;
            _daprClient = daprClient;
        }

        /// <summary>
        /// Confirms a new account using a confirmation token
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">If the request is aborted</param>
        /// <returns>A valid token if confirmed</returns>
        public async Task<Token> Handle(ValidateUserConfirmationCommand command, CancellationToken cancellationToken)
        {
            // Check email is valid
            var isValidEmail = command.Email.IsValidEmail();
            if (!isValidEmail)
                throw new UnprocessableEntityException("Email is not valid", FailureCode.EmailIsInvalid);

            // Check already exists
            var existingUser = await _identityUserService.FindByUserNameAsync(command.Email);
            if (existingUser == null)
                throw new NotFoundException("User already doesn't exist", FailureCode.UserDoesntExist);

            // Check not already confirmed
            if (existingUser.EmailConfirmed)
                throw new UnprocessableEntityException("Already confirmed", FailureCode.AlreadyConfirmed);

            // Confirm token is valid
            var confirmedToken = await _identityUserService.ValidateConfirmationTokenAsync(command.Email, command.ConfirmationToken);
            if (!confirmedToken)
                throw new ForbidException("Failed to validate confirmation token", FailureCode.FailedToValidateConfirmationToken);

            // Send email for user confirmation
            return await _identityUserService.BuildToken(command.Email);
        }
    }
}
