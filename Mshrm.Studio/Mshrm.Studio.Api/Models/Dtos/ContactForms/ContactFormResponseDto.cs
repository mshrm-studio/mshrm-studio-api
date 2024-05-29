using Mshrm.Studio.Api.Models.Dtos.User;
using Newtonsoft.Json;

namespace Mshrm.Studio.Api.Models.Dtos.ContactForms
{
    public class ContactFormResponseDto
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
        public string? ContactEmail { get; set; }

        /// <summary>
        /// The user who created the form
        /// </summary>
        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public MshrmStudioUserDto? User { get; set; }
    }
}
