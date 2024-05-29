using MediatR;
using Mshrm.Studio.Storage.Api.Models.Misc;

namespace Mshrm.Studio.Storage.Api.Models.CQRS.Resources.Queries
{
    public class GetResourceStreamQuery : IRequest<ResourceStream>
    {
        public Guid ResourceId { get; set; }
        public bool IsPrivate { get; set; }
    }
}
