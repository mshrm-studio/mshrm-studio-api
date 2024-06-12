using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Domain.AssetPriceHistories;

namespace Mshrm.Studio.Pricing.Api.Models.CQRS.AssetPriceHistories.Commands
{
    public class CreateAssetPriceHistoryCommand : IRequest<AssetPriceHistory>
    {
        /// <summary>
        /// The asset price
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The provider that updated the assets price from
        /// </summary>
        public PricingProviderType PricingProviderType { get; set; }

        /// <summary>
        /// The old price
        /// </summary>
        public decimal OldPrice { get; set; }

        /// <summary>
        /// The new price
        /// </summary>
        public decimal NewPrice { get; set; }

        /// <summary>
        /// The old market cap
        /// </summary>
        public decimal? OldMarketCap { get; set; }

        /// <summary>
        /// The new markey cap
        /// </summary>
        public decimal? NewMarketCap { get; set; }

        /// <summary>
        /// The old volume
        /// </summary>
        public decimal? OldVolume { get; set; }

        /// <summary>
        /// The new volume
        /// </summary>
        public decimal? NewVolume { get; set; }

        /// <summary>
        /// The date updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }
    }
}
