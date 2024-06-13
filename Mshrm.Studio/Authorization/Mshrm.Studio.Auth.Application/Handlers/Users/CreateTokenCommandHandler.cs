using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using System.Security.Claims;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using MediatR;
using Mshrm.Studio.Auth.Domain.Tokens.Commands;
using Mshrm.Studio.Auth.Application.Services.Interfaces;

namespace Mshrm.Studio.Auth.Application.Handlers.Users
{
    public class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, Token>
    {
        private readonly IIdentityUserService _identityUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTokenCommandHandler"/> class.
        /// </summary>
        /// <param name="identityUserService"></param>
        public CreateTokenCommandHandler(IIdentityUserService identityUserService)
        {
            _identityUserService = identityUserService;
        }

        /// <summary>
        /// Generate a token for credentials
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>Token</returns>
        /// <exception cref="ForbidException"></exception>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<Token> Handle(CreateTokenCommand command, CancellationToken cancellationToken)
        {
            // Check user exists
            var existingUser = await _identityUserService.FindByUserNameAsync(command.UserName);
            if (existingUser == null)
            {
                throw new ForbidException("Failed to generate token", FailureCode.FailedToGenerateToken);
            }

            // Check if the user is locked out
            var isLockedOut = await _identityUserService.IsUserLockedoutAsync(command.UserName);
            if (isLockedOut)
            {
                throw new ForbidException("User is locked out", FailureCode.UserLockedOut);
            }

            // Check user is confirmed
            if (existingUser.EmailConfirmed == false)
            {
                throw new ForbidException("User requires confirmation", FailureCode.UserRequiresConfirmation);
            }

            var token = await _identityUserService.RequestTokenAsync(command.UserName, command.Password);

            // Check we have a token generated otherwise something went wrong
            if (token == null)
                throw new UnauthorizedException("Failed to generate token", FailureCode.FailedToGenerateToken);

            // Otherwise all good
            return token;
        }
    }
}
