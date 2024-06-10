using Mshrm.Studio.Api.Clients;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Clients.Storage;
using Mshrm.Studio.Api.Models.Dtos.Files;
using Mshrm.Studio.Api.Services.Api.Interfaces;
using System.Collections.ObjectModel;

namespace Mshrm.Studio.Api.Services.Api
{
    public class CreateContactFormService : ICreateContactFormService
    {
        private readonly IDomainContactFormClient _contactFormClient;
        private readonly IFileClient _fileClient;

        public CreateContactFormService(IDomainContactFormClient contactFormClient, IFileClient fileClient) 
        {
            _contactFormClient = contactFormClient;
            _fileClient = fileClient;
        }

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
        public async Task<ContactFormDto> CreateContactFormAsync(string message, string contactEmail, string? firstName, string? lastName, string? websiteUrl, List<TemporaryFileRequestDto>? temporaryAttachmentKeys,
            CancellationToken cancellationToken)
        {
            var attachments = temporaryAttachmentKeys?.Select(x => new SaveTemporaryFileDto()
            {
                FileName = x.FileName,
                IsPrivate = true,
                Key = x.TemporaryKey,
            }).ToList();

            // Save the temp files
            var savedKeys = (attachments?.Any() ?? false) ? 
                (await _fileClient.SaveTemporaryFilesAsync(new SaveTemporaryFilesDto() { TemporaryFileKeys = new ObservableCollection<SaveTemporaryFileDto>(attachments) })).Select(x => x.GuidId.ToString()) : 
                new List<string>();

            // Create the contact form
            return await _contactFormClient.CreateContactFormAsync(new CreateContactFormDto()
            {
                ContactEmail = contactEmail,
                Message = message,
                FirstName = firstName,
                LastName = lastName,
                WebsiteUrl = websiteUrl,
                AttachmentKeys = new ObservableCollection<string>(savedKeys)
            });
        }
    }
}
