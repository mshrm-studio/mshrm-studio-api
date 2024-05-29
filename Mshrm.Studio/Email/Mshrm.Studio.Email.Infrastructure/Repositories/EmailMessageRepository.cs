using Mshrm.Studio.Email.Api.Context;
using Mshrm.Studio.Email.Api.Models.Entities;
using Mshrm.Studio.Email.Api.Repositories.Interfaces;
using Mshrm.Studio.Email.Domain.Emails;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Repositories.Bases;

namespace Mshrm.Studio.Email.Api.Repositories
{
    public class EmailMessageRepository : BaseRepository<EmailMessage, MshrmStudioEmailDbContext>, IEmailMessageRepository
    {
        private readonly IEmailMessageFactory _emailMessageFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public EmailMessageRepository(MshrmStudioEmailDbContext context, IEmailMessageFactory emailMessageFactory) : base(context)
        {
            _emailMessageFactory = emailMessageFactory;
        }

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
        public async Task<EmailMessage> CreateEmailMessageAsync(EmailType emailType, string toEmailAddress, string? subject, string? body, string? link, CancellationToken cancellationToken)
        {
            // Build email message
            var emailMessage = _emailMessageFactory.CreateEmailMessage(emailType, toEmailAddress, subject, body, link);

            // Save
            Add(emailMessage);
            await SaveAsync(cancellationToken);

            return emailMessage;
        }
    }
}
