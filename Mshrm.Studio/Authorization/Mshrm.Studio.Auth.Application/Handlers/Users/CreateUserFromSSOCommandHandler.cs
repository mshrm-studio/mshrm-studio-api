using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Auth.Api.Models.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Extensions;
using System.Data;
using RoleType = Mshrm.Studio.Auth.Api.Models.Enums.RoleType;
using Dapr.Client;
using System.Threading;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using System.Security.Claims;
using Mshrm.Studio.Shared.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
using Mshrm.Studio.Auth.Application.Services.Interfaces;
using Mshrm.Studio.Auth.Domain.User.Commands;

namespace Mshrm.Studio.Auth.Application.Handlers.Users
{
    /// <summary>
    /// Create a user
    /// </summary>
    public class CreateUserFromSSOCommandHandler : IRequestHandler<CreateUserFromSSOCommand, MshrmStudioIdentityUser>
    {
        private readonly ILogger<CreateUserCommand> _logger;
        private readonly IIdentityUserService _identityUserService;
        private readonly DaprClient _daprClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserCommand"/> class.
        /// </summary>
        /// <param name="identityUserService"></param>
        /// <param name="daprClient"></param>
        /// <param name="logger"></param>
        public CreateUserFromSSOCommandHandler(IIdentityUserService identityUserService, DaprClient daprClient, ILogger<CreateUserCommand> logger)
        {
            _logger = logger;
            _identityUserService = identityUserService;
            _daprClient = daprClient;
        }

        /// <summary>
        /// Create a new user using the SSO token
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The new user</returns>
        public async Task<MshrmStudioIdentityUser> Handle(CreateUserFromSSOCommand command, CancellationToken cancellationToken)
        {
            var email = command.User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                throw new NotFoundException("Email doesn't exist", FailureCode.EmailNotFound);
            }

            var firstName = command.User.FindFirstValue("given_name") ?? email;
            var lastName = command.User.FindFirstValue("family_name") ?? string.Empty;

            // Check already exists
            var existingUser = await _identityUserService.FindByUserNameAsync(email);
            if (existingUser != null)
            {
                throw new UnprocessableEntityException("User already exists", FailureCode.UserAlreadyExists);
            }

            // Check IP is valid so we can log against the requesting user
            if (string.IsNullOrEmpty(command.IPAddress))
            {
                throw new NotFoundException("The IP address provided is not valid", FailureCode.IpNotValid);
            }

            // Otherwise create new user (set as random password since user is using SSO and has not set one yet)
            var identityUser = await _identityUserService.CreateIdentityUserAsync(email, firstName, lastName, StringUtility.RandomString(12), command.IPAddress, new List<RoleType>() { RoleType.User }, true);
            if (identityUser == null)
            {
                throw new UnprocessableEntityException("Failed to create user", FailureCode.FailedToCreateIdentityUser);
            }

            return identityUser;
        }
    }
}
