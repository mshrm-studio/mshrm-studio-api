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
    public class CreateUserFromSSOCommand : IRequest<MshrmStudioIdentityUser>
    {
        /// <summary>
        /// The user from claims
        /// </summary>
        public ClaimsPrincipal User { get; set; }

        /// <summary>
        /// The callers IP
        /// </summary>
        public string? IPAddress { get; set; }
    }
}
