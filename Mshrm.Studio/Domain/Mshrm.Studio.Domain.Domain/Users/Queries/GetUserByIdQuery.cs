using MediatR;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Domain.Api.Models.CQRS.Users.Queries
{
    public class GetUserByIdQuery : IRequest<User>
    {
        public int Id { get; set; }
    }
}
