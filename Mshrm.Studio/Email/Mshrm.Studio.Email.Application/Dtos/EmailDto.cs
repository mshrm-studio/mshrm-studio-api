using Mshrm.Studio.Shared.Enums;
using Newtonsoft.Json;

namespace Mshrm.Studio.Email.Api.Models.Dtos
{
    /// <summary>
    /// Email
    /// </summary>
    public class EmailDto
    {
        /// <summary>
        /// The type of email
        /// </summary>
        [JsonProperty("emailType")]
        public EmailType EmailType { get; set; }

        /// <summary>
        /// Who the emails to
        /// </summary>
        [JsonProperty("toEmailAddress")]
        public required string ToEmailAddress { get; set; }

        /// <summary>
        /// A link for the email button
        /// </summary>
        [JsonProperty("link")]
        public string? Link { get; set; }

        /// <summary>
        /// Any values to replace in text
        /// </summary>
        [JsonProperty("replaceValues")]
        public List<KeyValuePair<string, string>>? ReplaceValues { get; set; }
    }
}
