using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Auth.Api.Models.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Extensions;
using System.Data;
using RoleType = Mshrm.Studio.Auth.Api.Models.Enums.RoleType;
using System.Threading;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using System.Security.Claims;
using Mshrm.Studio.Shared.Helpers;
using MediatR;

namespace Mshrm.Studio.Auth.Domain.User.Commands
{
    /// <summary>
    /// Create a user
    /// </summary>
    public class CreateUserAnyRoleCommand : IRequest<MshrmStudioIdentityUser>
    {
        /// <summary>
        /// The users email
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The users first name
        /// </summary>
        public required string FirstName { get; set; }

        /// <summary>
        /// The users last name
        /// </summary>
        public required string LastName { get; set; }

        /// <summary>
        /// The users password
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// The users role
        /// </summary>
        public RoleType Role { get; set; }
    }
}
