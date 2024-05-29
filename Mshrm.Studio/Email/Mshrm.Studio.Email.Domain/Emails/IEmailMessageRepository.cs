using Mshrm.Studio.Email.Api.Models.Entities;
using Mshrm.Studio.Shared.Enums;

namespace Mshrm.Studio.Email.Api.Repositories.Interfaces
{
    public interface IEmailMessageRepository
    {
        /// <summary>
        /// Create a new email message
        /// </summary>
        /// <param name="emailType">The type of email the message is for</param>
        /// <param name="toEmailAddress">The address the email was sent to</param>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="body">The emails content</param>
        /// <param name="link">The link sent</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A new email message</returns>
        public Task<EmailMessage> CreateEmailMessageAsync(EmailType emailType, string toEmailAddress, string? subject, string? body, string? link, CancellationToken cancellationToken);
    }
}
