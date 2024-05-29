
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using System.Threading;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using MediatR;
using Mshrm.Studio.Auth.Api.Models.Entities;

namespace Mshrm.Studio.Auth.Domain.User.Commands
{
    /// <summary>
    /// For updating a users password
    /// </summary>
    public class ResetPasswordCommand : IRequest<bool>
    {
        /// <summary>
        /// The email to reset pw
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The token
        /// </summary>
        public required string Token { get; set; }

        /// <summary>
        /// The new password
        /// </summary>
        public required string NewPassword { get; set; }
    }
}
