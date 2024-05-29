using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Auth.Api.Models.Dtos
{
    public class UpdatePasswordDto
    {
        /// <summary>
        /// The password to set for new user. 
        /// Example: Pajhdy66_oRd
        /// </summary>
        [JsonProperty("oldPassword")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{10,}$", ErrorMessage = "Passwords must be at least 10 characters and contain all of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string OldPassword { get; set; }

        /// <summary>
        /// The password to set for new user. 
        /// Example: Pajhdy66_oRd
        /// </summary>
        [JsonProperty("newPassword")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{10,}$", ErrorMessage = "Passwords must be at least 10 characters and contain all of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string NewPassword { get; set; }
    }
}
