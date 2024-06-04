using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Domain.Api.Models.Events;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Models.Entities.Bases;
using Mshrm.Studio.Shared.Models.Entities.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mshrm.Studio.Domain.Api.Models.Entity
{
    /// <summary>
    /// A contact form
    /// </summary>
    [Index("GuidId", "ContactEmail")]
    public class ContactForm : AuditableEntity, IAggregateRoot
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
        /// A contact email for the message
        /// </summary>
        public string ContactEmail { get; private set; }

        /// <summary>
        /// The message sent
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// First name
        /// </summary>
        public string? FirstName { get; private set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string? LastName { get; private set; }

        /// <summary>
        /// Any website URL to link to
        /// </summary>
        public string? WebsiteUrl { get; private set; }

        /// <summary>
        /// Any contact form attachments
        /// </summary>
        public List<Guid> AttachmentGuidIds { get; private set; } = new List<Guid>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">The message sent</param>
        /// <param name="contactEmail"></param>
        public ContactForm(string message, string contactEmail, string? firstName, string? lastName, string? websiteUrl, List<Guid> attachmentGuidIds)
        {
            // Check email exists
            if (!(contactEmail?.IsValidEmail() ?? false))
            {
                throw new UnprocessableEntityException("Email is invalid", FailureCode.EmailIsInvalid);
            }

            Message = message;
            ContactEmail = contactEmail.ToLower().Trim();
            FirstName = firstName;
            LastName = lastName;
            WebsiteUrl = websiteUrl;

            if (attachmentGuidIds.Any())
            {
                AttachmentGuidIds.AddRange(attachmentGuidIds);
            }
        }
    }
}
