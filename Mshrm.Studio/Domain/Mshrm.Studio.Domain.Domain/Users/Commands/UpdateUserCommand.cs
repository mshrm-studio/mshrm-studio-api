using MediatR;
using Mshrm.Studio.Auth.Api.Models.Enums;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Shared.Models.Pagination;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Domain.Api.Models.CQRS.Users.Commands
{
    public class UpdateUserCommand : IRequest<User>
    {
        /// <summary>
        /// The user to update
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The users email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The users first name
        /// </summary>
        public required string FirstName { get; set; }

        /// <summary>
        /// The users last name
        /// </summary>
        public required string LastName { get; set; }

        /// <summary>
        /// The callers role type
        /// </summary>
        public RoleType CallingUsersRoleType { get; set; }

        /// <summary>
        /// The callers email
        /// </summary>
        public string CallingUsersEmail { get; set; }

        /// <summary>
        /// The requesting users IP
        /// </summary>
        public string? Ip { get; set; }
    }
}
