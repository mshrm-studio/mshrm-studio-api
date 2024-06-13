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
    public class CreateRefreshTokenCommandHandler : IRequestHandler<CreateRefreshTokenCommand, Token>
    {
        private readonly IIdentityUserService _identityUserService;

        public CreateRefreshTokenCommandHandler(IIdentityUserService identityUserService)
        {
            _identityUserService = identityUserService;
        }

        /// <summary>
        /// Generate a refresh token
        /// </summary>
        /// <param name="command">The token to generate from</param>
        /// <returns>A new valid JWT token</returns>
        public async Task<Token> Handle(CreateRefreshTokenCommand command, CancellationToken cancellationToken)
        {
            // Check we even have a refresh token
            if (string.IsNullOrEmpty(command.RefreshToken))
            {
                throw new ForbidException("Failed to refresh token", FailureCode.FailedToRefreshToken);
            }

            // Get the user
            var principal = _identityUserService.GetPrincipalFromExpiredToken(command.Token);
            var username = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? string.Empty;
            if (string.IsNullOrEmpty(username))
            {
                throw new BadRequestException("Username doesn't exist", FailureCode.UserDoesntExist);
            }

            // Find user
            var user = await _identityUserService.FindByUserNameAsync(username);
            if (user == null)
            {
                throw new ForbidException("Failed to refresh token", FailureCode.FailedToRefreshToken);
            }

            // Check the refresh token
            if (user == null || user.RefreshToken != command.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedException("Failed to refresh token", FailureCode.FailedToRefreshToken);
            }
            // Get a new token/access token
            return await _identityUserService.BuildToken(username);
        }
    }
}
