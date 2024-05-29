
using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Domain;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface ICreateContactFormService
    {
        /// <summary>
        /// Create a new contact form
        /// </summary>
        /// <param name="message">The message sent</param>
        /// <param name="contactEmail">A contact email for the message</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The new contact form</returns>
        Task<ContactFormDto> CreateContactFormAsync(string message, string contactEmail, CancellationToken cancellationToken);
    }
}
