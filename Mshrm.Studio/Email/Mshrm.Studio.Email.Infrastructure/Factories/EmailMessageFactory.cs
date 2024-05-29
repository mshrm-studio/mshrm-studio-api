using Mshrm.Studio.Email.Api.Models.Entities;
using Mshrm.Studio.Email.Domain.Emails;
using Mshrm.Studio.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Email.Infrastructure.Factories
{
    public class EmailMessageFactory : IEmailMessageFactory
    {
        /// <summary>
        /// Create a new email message
        /// </summary>
        /// <param name="emailType">The type of email being sent</param>
        /// <param name="emailSentTo">Who to send email to</param>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="content">The email body</param>
        /// <param name="link">The URL the email button links to</param>
        /// <returns>A new email message</returns>
        public EmailMessage CreateEmailMessage(EmailType emailType, string emailSentTo, string? subject, string? content, string? link)
        {
            return new EmailMessage(emailType, emailSentTo, subject, content, link);
        }
    }
}
