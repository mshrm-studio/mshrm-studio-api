using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Pricing.Api.Models.CQRS.Assets.Queries
{
    public class GetPagedAssetsQuery : IRequest<PagedResult<Asset>>
    {
        public string? Search { get; set; }
        public string? Symbol { get; set; }
        public string? Name { get; set; }
        public PricingProviderType? PricingProviderType { get; set; }
        public AssetType? AssetType { get; set; }
        public string OrderProperty { get; set; }
        public Order Order { get; set; }
        public uint PageNumber { get; set; }
        public uint PerPage { get; set; }
    }
}
