using MediatR;
using Mshrm.Studio.Domain.Api.Models.CQRS.Shared;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Models.Enums;
using Mshrm.Studio.Shared.Enums;

namespace Mshrm.Studio.Domain.Api.Models.CQRS.Users.Queries
{
    public class GetUserByGuidQuery : BaseGuidQuery, IRequest<User>
    {
    }
}
