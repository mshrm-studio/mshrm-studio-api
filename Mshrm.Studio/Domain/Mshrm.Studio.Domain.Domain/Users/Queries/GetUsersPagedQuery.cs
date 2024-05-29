using MediatR;
using Mshrm.Studio.Domain.Api.Models.CQRS.Shared;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Domain.Api.Models.CQRS.Users.Queries
{
    public class GetUsersPagedQuery : PagedQuery, IRequest<PagedResult<User>>
    {
        public string? SearchTerm { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
    }
}
