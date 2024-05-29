
using Mshrm.Studio.Shared.Enums;

namespace Mshrm.Studio.Email.Api.Models.Dtos
{
    /// <summary>
    /// An email message
    /// </summary>
    public class EmailMessageDto
    {
        /// <summary>
        /// The users ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A guid version of the integer ID
        /// </summary>
        public Guid GuidId { get; set; }

        /// <summary>
        /// The type of email being sent
        /// </summary>
        public EmailType EmailType { get; set; }

        /// <summary>
        /// The link URL for the email being sent
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// The content of email being sent
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// The link URL for the email being sent
        /// </summary>
        public string? Link { get; set; }

        /// <summary>
        /// The email the mail is sent to
        /// </summary>
        public required string EmailSentTo { get; set; }
    }
}
