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
using Mshrm.Studio.Auth.Domain.User.Commands;
using Mshrm.Studio.Auth.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Mshrm.Studio.Auth.Api.Handlers
{
    /// <summary>
    /// Create a user
    /// </summary>
    public class CreateUserAnyRoleCommandHandler : IRequestHandler<CreateUserAnyRoleCommand, MshrmStudioIdentityUser>
    {
        private readonly ILogger<CreateUserAnyRoleCommandHandler> _logger;
        private readonly IIdentityUserService _identityUserService;
        private readonly DaprClient _daprClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserCommand"/> class.
        /// </summary>
        /// <param name="identityUserService"></param>
        /// <param name="daprClient"></param>
        /// <param name="logger"></param>
        public CreateUserAnyRoleCommandHandler(IIdentityUserService identityUserService, DaprClient daprClient, ILogger<CreateUserAnyRoleCommandHandler> logger)
        {
            _logger = logger;
            _identityUserService = identityUserService;
            _daprClient = daprClient;
        }

        /// <summary>
        /// Create a new identity user with any role
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">If the request is aborted</param>
        /// <returns>The new identity user</returns>
        public async Task<MshrmStudioIdentityUser> Handle(CreateUserAnyRoleCommand command, CancellationToken cancellationToken)
        {
            // Check email is valid
            var isValidEmail = command.Email.IsValidEmail();
            if (!isValidEmail)
                throw new UnprocessableEntityException("Email is not valid", FailureCode.EmailIsInvalid);

            // Check already exists
            var existingUser = await _identityUserService.FindByUserNameAsync(command.Email);
            if (existingUser != null)
                throw new UnprocessableEntityException("User already exists", FailureCode.UserAlreadyExists);

            // Otherwise create new user
            var newIdentityUser = await _identityUserService.CreateIdentityUserAsync(command.Email, command.FirstName, command.LastName, command.Password, null, new List<RoleType>() { command.Role }, true);

            return newIdentityUser;
        }
    }
}
