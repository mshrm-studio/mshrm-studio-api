
using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Models.Dtos.Files;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface ICreateContactFormService
    {
        /// <summary>
        /// Create a new contact form
        /// </summary>
        /// <param name="message">The message sent</param>
        /// <param name="contactEmail">A contact email for the message</param>
        /// <param name="firstName">User creating first name</param>
        /// <param name="lastName">User creating last name</param>
        /// <param name="websiteUrl">A linked website</param>
        /// <param name="temporaryAttachmentKeys">Attachments</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The new contact form</returns>
        Task<ContactFormDto> CreateContactFormAsync(string message, string contactEmail, string? firstName, string? lastName, string? websiteUrl, List<TemporaryFileRequestDto>? temporaryAttachmentKeys,
            CancellationToken cancellationToken);
    }
}
