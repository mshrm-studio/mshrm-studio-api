
using MediatR;
using Mshrm.Studio.Domain.Api.Models.Entity;

namespace Mshrm.Studio.Domain.Api.Models.CQRS.Users.Queries
{
    public class GetUserByEmailQuery : IRequest<User>
    {
        public required string Email { get; set; }
    }
}
