using MediatR;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Auth.Api.Models.Enums;
using Mshrm.Studio.Auth.Api.Models.Pocos;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;

namespace Mshrm.Studio.Auth.Domain.User.Queries
{
    /// <summary>
    /// Service to query users (read only)
    /// </summary>
    public class GetUserByEmailQuery : IRequest<MshrmStudioUser>
    {
        /// <summary>
        /// The email
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The person requestings email
        /// </summary>
        public required string RequestingUsersEmail { get; set; }

        /// <summary>
        /// The person requestings role
        /// </summary>
        public RoleType RequestingUsersRole { get; set; }
    }
}
