using Mshrm.Studio.Domain.Api.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Domain.Domain.ContactForms
{
    public interface IContactFormFactory
    {
        /// <summary>
        /// Create a new contact form
        /// </summary>
        /// <param name="userId">The user making the request</param>
        /// <param name="message">The message sent</param>
        /// <param name="contactEmail"></param>
        /// <returns>A new contact form</returns>
        public ContactForm CreateContactForm(string message, string contactEmail, string? firstName, string? lastName, string? websiteUrl, List<Guid> attachmentGuidIds);
    }
}
