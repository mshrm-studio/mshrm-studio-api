using Mshrm.Studio.Shared.Enums;

namespace Mshrm.Studio.Domain.Api.Models.CQRS.Shared
{
    public class PagedQuery
    {
        public Order Order { get; set; }
        public string? OrderProperty { get; set; }
        public uint PageNumber { get; set; }
        public uint PerPage { get; set; }
    }
}
