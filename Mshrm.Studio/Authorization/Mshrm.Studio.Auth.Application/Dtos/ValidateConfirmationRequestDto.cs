using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Auth.Api.Models.Dtos
{
    public class ValidateConfirmationRequestDto
    {
        /// <summary>
        /// The user to registered email address this cannot be updated (used for identification). 
        /// Example: test@test.com
        /// </summary>
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,50})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("confirmationToken")]
        public string ConfirmationToken { get; set; }
    }
}
