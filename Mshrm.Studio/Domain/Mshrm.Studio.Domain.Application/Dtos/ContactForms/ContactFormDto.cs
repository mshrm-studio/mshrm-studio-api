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

        /// <summary>
        /// First name
        /// </summary>
        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        [JsonProperty("lastName")]
        public string? LastName { get; set; }

        /// <summary>
        /// Any website URL to link to
        /// </summary>
        [JsonProperty("websiteUrl")]
        public string? WebsiteUrl { get; set; }

        /// <summary>
        /// Any contact form attachments guid ids
        /// </summary>
        [JsonProperty("attachmentGuidIds")]
        public List<Guid> AttachmentGuidIds { get; set; } = new List<Guid>();

        /// <summary>
        /// Any contact form attachment URLs
        /// </summary>
        [JsonProperty("attachmentUrls")]
        public List<string> AttachmentUrls { get; set; } = new List<string>();
    }
}
