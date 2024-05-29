using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Domain.Api.Models.Dtos.Users
{
    /// <summary>
    /// For creating new users with the USER role
    /// </summary>
    public class CreateDomainUserDto
    {
        /// <summary>
        /// An email indetification for a user
        /// </summary>
        [JsonProperty("email")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,50})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$")]
        public required string Email { get; set; }

        /// <summary>
        /// The first name of a user
        /// </summary>
        [JsonProperty("firstName")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        public required string FirstName { get; set; }

        /// <summary>
        /// The last name of a user
        /// </summary>
        [JsonProperty("lastName")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        public required string LastName { get; set; }

        /// <summary>
        /// The IP of the requesting user
        /// </summary>
        [JsonProperty("ip")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        public string? Ip { get; set; }

        /// <summary>
        /// The active state of the user
        /// </summary>
        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
