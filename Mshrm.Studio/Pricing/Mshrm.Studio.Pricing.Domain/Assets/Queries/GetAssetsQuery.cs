using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Models.CQRS.Assets.Queries
{
    public class GetAssetsQuery : IRequest<List<Asset>>
    {
        public PricingProviderType PricingProviderType { get; set; }
        public AssetType? AssetType { get; set; }
        public List<string>? Symbols { get; set; }
        public bool? Active { get; set; }
    }
}
