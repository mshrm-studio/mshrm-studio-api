using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Services.Api.Interfaces;

namespace Mshrm.Studio.Api.Services.Api
{
    public class CreateContactFormService : ICreateContactFormService
    {
        private readonly IDomainContactFormClient _contactFormClient;

        public CreateContactFormService(IDomainContactFormClient contactFormClient) 
        {
            _contactFormClient = contactFormClient;
        }

        /// <summary>
        /// Create a new contact form
        /// </summary>
        /// <param name="message">The message sent</param>
        /// <param name="contactEmail">A contact email for the message</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The new contact form</returns>
        public async Task<ContactFormDto> CreateContactFormAsync(string message, string contactEmail, CancellationToken cancellationToken)
        {
            return await _contactFormClient.CreateContactFormAsync(new CreateContactFormDto()
            {
                ContactEmail = contactEmail,
                Message = message,
            });
        }
    }
}
