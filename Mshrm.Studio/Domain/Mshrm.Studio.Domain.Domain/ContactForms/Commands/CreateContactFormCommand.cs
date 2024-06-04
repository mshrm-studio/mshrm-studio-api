using MediatR;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Domain.Api.Models.CQRS.ContactForms.Commands
{
    public class CreateContactFormCommand : IRequest<ContactForm>
    {
        public required string Message { get; set; }
        public Guid UserId { get; set; }
        public string ContactEmail { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? WebsiteUrl { get; set; }
        public List<string> TemporaryAttachmentIds { get; set; } = new List<string>();
    }
}
