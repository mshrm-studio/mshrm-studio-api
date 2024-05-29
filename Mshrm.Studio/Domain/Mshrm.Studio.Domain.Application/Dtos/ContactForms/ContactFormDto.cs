using Mshrm.Studio.Domain.Api.Models.Dtos.Users;
using Newtonsoft.Json;

namespace Mshrm.Studio.Domain.Api.Models.Dtos.ContactForms
{
    public class ContactFormDto
    {
        /// <summary>
        /// A guid version of the integer ID
        /// </summary>
        [JsonProperty("guidId")]
        public Guid GuidId { get; set; }

        /// <summary>
        /// The message sent
        /// </summary>
        [JsonProperty("message")]
        public required string Message { get; set; }

        /// <summary>
        /// A contact email for the message
        /// </summary>
        [JsonProperty("contactEmail")]
        public required string ContactEmail { get; set; }
    }
}
