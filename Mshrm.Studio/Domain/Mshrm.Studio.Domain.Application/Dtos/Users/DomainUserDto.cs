using Mshrm.Studio.Domain.Api.Models.Events;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Mshrm.Studio.Domain.Api.Models.Dtos.Users
{
    /// <summary>
    /// User model
    /// </summary>
    public class DomainUserDto
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
        public required string Email { get; set; } = string.Empty;

        /// <summary>
        /// The first name of a user
        /// </summary>
        [JsonProperty("firstName")]
        public required string FirstName { get; set; }

        /// <summary>
        /// The last name of a user
        /// </summary>
        [JsonProperty("lastName")]
        public required string LastName { get; set; }

        /// <summary>
        /// The full name name of a user (aggregation of first and last name)
        /// </summary>
        [JsonProperty("fullName")]
        public required string FullName { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DomainUserDto()
        {
        }
    }
}
