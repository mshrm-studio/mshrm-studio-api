using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Models.CQRS.AssetPrices.Queries
{
    public class GetLatestPricesQuery : IRequest<List<AssetPrice>>
    {
        public List<string>? Symbols { get; set; }
        public string BaseAssetSymbol { get; set; }
        public AssetType? AssetType { get; set; }
        public PricingProviderType? PricingProviderType { get; set; }
    }
}
