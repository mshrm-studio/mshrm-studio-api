using Mshrm.Studio.Auth.Api.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Auth.Application.Dtos.Users
{
    public class IdentityUserResponseDto
    {
        /// <summary>
        /// The user to registered email address this cannot be updated (used for identification). 
        /// Example: test@test.com
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// The users role. 
        /// Example: Member
        /// </summary>
        [JsonProperty("role")]
        public RoleType Role { get; set; }

        /// <summary>
        /// If the user has been confirmed
        /// </summary>
        [JsonProperty("confirmed")]
        public bool Confirmed { get; set; }
    }
}
