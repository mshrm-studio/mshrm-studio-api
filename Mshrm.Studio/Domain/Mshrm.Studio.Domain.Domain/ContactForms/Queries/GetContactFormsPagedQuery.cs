using MediatR;
using Mshrm.Studio.Domain.Api.Models.CQRS.Shared;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Models.Enums;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Domain.Api.Models.CQRS.ContactForms.Queries
{
    public class GetContactFormsPagedQuery: PagedQuery, IRequest<PagedResult<ContactForm>>
    {
        public string? SearchTerm { get; set; }
        public string? ContactEmail { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }
}
