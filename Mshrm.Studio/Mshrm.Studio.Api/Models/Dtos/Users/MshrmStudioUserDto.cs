using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Auth;
using Newtonsoft.Json;

namespace Mshrm.Studio.Api.Models.Dtos.User
{
    public class MshrmStudioUserDto
    {
        /// <summary>
        /// A guid version of the integer ID
        /// </summary>
        [JsonProperty("guidId")]
        public Guid GuidId { get; set; }

        /// <summary>
        /// An email indetification for a user
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The first name of a user
        /// </summary>
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of a user
        /// </summary>
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// The full name name of a user (aggregation of first and last name)
        /// </summary>
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        /// <summary>
        /// If the user is confirmed or not
        /// </summary>
        [JsonProperty("confirmed")]
        public bool Confirmed { get; set; }

        /// <summary>
        /// The users role
        /// </summary>
        [JsonProperty("role")]
        public RoleType RoleType { get; set; }
    }
}
