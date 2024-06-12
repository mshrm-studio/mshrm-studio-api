using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Domain.AssetPriceHistories.Queries
{
    public class GetPagedAssetPriceHistoryQuery : IRequest<PagedResult<AssetPriceHistory>>
    {
        public string AssetGuidId { get; set; }
        public string BaseAssetGuidId { get; set; }
        public PricingProviderType? PricingProviderType { get; set; }
        public string OrderProperty { get; set; }
        public Order Order { get; set; }
        public uint PageNumber { get; set; }
        public uint PerPage { get; set; }
    }
}
