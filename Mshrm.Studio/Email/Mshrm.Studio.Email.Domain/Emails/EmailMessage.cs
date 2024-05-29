using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Models.Entities.Bases;
using Mshrm.Studio.Shared.Models.Entities.Interfaces;

namespace Mshrm.Studio.Email.Api.Models.Entities
{
    [Index("GuidId")]
    public class EmailMessage : AuditableEntity, IAggregateRoot
    {
        /// <summary>
        /// The users ID
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// A guid version of the integer ID
        /// </summary>
        public Guid GuidId { get; private set; }

        /// <summary>
        /// The type of email being sent
        /// </summary>
        public EmailType EmailType { get; private set; }

        /// <summary>
        /// The link URL for the email being sent
        /// </summary>
        public string? Subject { get; private set; }

        /// <summary>
        /// The content of email being sent
        /// </summary>
        public string? Content { get; private set; }

        /// <summary>
        /// The link URL for the email being sent
        /// </summary>
        public string? Link { get; private set; }

        /// <summary>
        /// The email the mail is sent to
        /// </summary>
        public string EmailSentTo { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="emailType">The type of email being sent</param>
        /// <param name="emailSentTo">Who to send email to</param>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="content">The email body</param>
        /// <param name="link">The URL the email button links to</param>
        public EmailMessage(EmailType emailType, string emailSentTo, string? subject, string? content, string? link)
        {
            EmailType = emailType;
            EmailSentTo = emailSentTo;
            Subject = subject;
            Content = content;
            Link = link;
        }
    }
}
