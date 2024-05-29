using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Api.Models.Dtos.ContactForm
{
    public class CreateNewContactFormDto
    {
        /// <summary>
        /// The message sent
        /// </summary>
        [JsonProperty("message")]
        [StringLength(10000, ErrorMessage = "{0} must have less than {1} characters")]
        public string Message { get; set; }

        /// <summary>
        /// A contact email for the message
        /// </summary>
        [JsonProperty("contactEmail")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,50})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$")]
        public required string ContactEmail { get; set; }
    }
}
