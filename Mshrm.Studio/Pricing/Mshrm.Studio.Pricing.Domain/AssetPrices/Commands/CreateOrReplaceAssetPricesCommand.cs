using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Models.CQRS.AssetPrices.Commands
{
    public class CreateOrReplaceAssetPricesCommand : IRequest<List<AssetPrice>>
    {
        public List<PricePair> Prices { get; set; }
        public PricingProviderType PricingProviderType { get; set; }
    }
}
