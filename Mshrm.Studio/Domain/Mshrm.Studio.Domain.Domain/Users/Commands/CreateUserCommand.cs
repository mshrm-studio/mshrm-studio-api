using MediatR;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Shared.Models.Pagination;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Domain.Api.Models.CQRS.Users.Commands
{
    public class CreateUserCommand : IRequest<User>
    {
        /// <summary>
        /// The user to registered email address this cannot be updated (used for identification). 
        /// Example: test@test.com
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The password to set for new user. 
        /// Example: Pajhdy66_oRd
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// The users first name
        /// </summary>
        public required string FirstName { get; set; }

        /// <summary>
        /// The users last name
        /// </summary>
        public required string LastName { get; set; }

        /// <summary>
        /// The users IP
        /// </summary>
        public string? Ip { get; set; }
    }
}
