using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using System.Threading;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using MediatR;

namespace Mshrm.Studio.Auth.Domain.User.Commands
{
    /// <summary>
    /// For updating a users password
    /// </summary>
    public class CreatePasswordResetTokenCommand : IRequest
    {
        /// <summary>
        /// The email to send to
        /// </summary>
        public required string Email { get; set; }
    }
}
