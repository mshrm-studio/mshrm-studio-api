using Mshrm.Studio.Auth.Api.Models.Enums;

namespace Mshrm.Studio.Auth.Api.Models.Pocos
{
    public class MshrmStudioUser
    {
        /// <summary>
        /// The user to registered email address this cannot be updated (used for identification). 
        /// Example: test@test.com
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The users role. 
        /// Example: Member
        /// </summary>
        public RoleType Role { get; set; }

        /// <summary>
        /// If the user has been confirmed
        /// </summary>
        public bool Confirmed { get; set; }
    }
}
