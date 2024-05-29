using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using System.Security.Claims;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using MediatR;

namespace Mshrm.Studio.Auth.Domain.Tokens.Commands
{
    public class CreateTokenCommand : IRequest<Token>
    {
        /// <summary>
        /// The command username
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// The command password
        /// </summary>
        public required string Password { get; set; }
    }
}
