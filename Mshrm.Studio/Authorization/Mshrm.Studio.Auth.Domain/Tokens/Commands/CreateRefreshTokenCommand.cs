using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using System.Security.Claims;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using MediatR;

namespace Mshrm.Studio.Auth.Domain.Tokens.Commands
{
    public class CreateRefreshTokenCommand : IRequest<Token>
    {
        /// <summary>
        /// A valid refresh token
        /// </summary>
        public required string RefreshToken { get; set; }

        /// <summary>
        /// An expired token
        /// </summary>
        public required string Token { get; set; }
    }
}
