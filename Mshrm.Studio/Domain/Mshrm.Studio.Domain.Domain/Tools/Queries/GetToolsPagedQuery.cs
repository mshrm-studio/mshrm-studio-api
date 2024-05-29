using MediatR;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Models.Enums;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Domain.Api.Models.CQRS.Tools.Queries
{
    public class GetToolsPagedQuery : IRequest<PagedResult<Tool>>
    {
        public string? SearchTerm { get; set; }
        public string? Name { get; set; }
        public ToolType? ToolType { get; set; }
        public Order Order { get; set; }
        public string? OrderProperty { get; set; }
        public uint PageNumber { get; set; }
        public uint PerPage { get; set; }
    }
}
