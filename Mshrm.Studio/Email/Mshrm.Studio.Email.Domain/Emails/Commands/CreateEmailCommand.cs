using MediatR;
using Mshrm.Studio.Email.Api.Models.Entities;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Email.Api.Models.CQRS.Emails.Commands
{
    public class CreateEmailCommand : IRequest<EmailMessage>
    {
        /// <summary>
        /// The type of email
        /// </summary>
        public EmailType EmailType { get; set; }

        /// <summary>
        /// Who the emails to
        /// </summary>
        public required string ToEmailAddress { get; set; }

        /// <summary>
        /// A link for the email button
        /// </summary>
        public string? Link { get; set; }

        /// <summary>
        /// Any values to replace in text
        /// </summary>
        public List<KeyValuePair<string, string>>? ReplaceValues { get; set; }
    }
}
