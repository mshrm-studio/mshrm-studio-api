using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Models.CQRS.AssetPrices.Events
{
    public class AssetPriceCreatedEvent : INotification
    {
        public AssetPrice AssetPrice { get; }

        public AssetPriceCreatedEvent(AssetPrice assetPrice)
        {
            AssetPrice = assetPrice;
        }
    }
}
