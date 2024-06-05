using AutoMapper;
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Assets;
using Mshrm.Studio.Api.Models.Dtos.ContactForms;
using System.Runtime.Intrinsics;

namespace Mshrm.Studio.Api.Mapping.Converters
{
    /// <summary>
    /// For the conversion of contactFormDTO to contactFormResponseDTO
    /// </summary>
    public class ContactFormConverter : ITypeConverter<ContactFormDto, ContactFormResponseDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactFormConverter"/> class.
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public ContactFormConverter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Convert a ContactFormDto to a ContactFormResponseDto
        /// </summary>
        /// <param name="parent">The ContactFormDto</param>
        /// <param name="target">The ContactFormResponseDto</param>
        /// <param name="context">The request context</param>
        /// <returns>A ContactFormResponseDto</returns>
        public ContactFormResponseDto Convert(
            ContactFormDto parent,
            ContactFormResponseDto target,
            ResolutionContext context)
        {
            target = new ContactFormResponseDto() { Message = string.Empty };

            // Build the urls
            var urls = (parent.AttachmentGuidIds?.Any() ?? false) ? parent.AttachmentGuidIds.Select(x => $"https://{_httpContextAccessor.HttpContext?.Request.Host}/api/v1/files/{x}").ToList() : null;

            // Set the rest of the properties
            target.GuidId = parent.GuidId;
            target.Message = parent.Message;
            target.ContactEmail = parent.ContactEmail;
            target.FirstName = parent.FirstName;
            target.LastName = parent.LastName;
            target.WebsiteUrl = parent.WebsiteUrl;
            target.AttachmentGuidIds = parent.AttachmentGuidIds?.ToList() ?? new List<Guid>();
            target.AttachmentUrls = urls ?? new List<string>();

            // Return mapped target
            return target;
        }
    }
}
