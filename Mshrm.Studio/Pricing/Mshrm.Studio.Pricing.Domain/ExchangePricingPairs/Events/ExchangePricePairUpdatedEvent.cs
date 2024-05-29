using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Models.Events
{
    public class ExchangePricePairUpdatedEvent : INotification
    {
        /// <summary>
        /// The exchange pair updated
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The provider that updated the pair
        /// </summary>
        public PricingProviderType PricingProviderType { get; private set; }

        /// <summary>
        /// The old price
        /// </summary>
        public decimal OldPrice { get; private set; }

        /// <summary>
        /// The new price
        /// </summary>
        public decimal NewPrice { get; private set; }

        /// <summary>
        /// The old market cap
        /// </summary>
        public decimal? OldMarketCap { get; private set; }

        /// <summary>
        /// The new markey cap
        /// </summary>
        public decimal? NewMarketCap { get; private set; }

        /// <summary>
        /// The old volume
        /// </summary>
        public decimal? OldVolume { get; private set; }

        /// <summary>
        /// The new volume
        /// </summary>
        public decimal? NewVolume { get; private set; }

        /// <summary>
        /// The date updated
        /// </summary>
        public DateTime UpdatedDate { get; private set; }

        /// <summary>
        /// Constructtor
        /// </summary>
        /// <param name="id">The tools id</param>
        /// <param name="pricingProviderType"></param>
        /// <param name="oldPrice"></param>
        /// <param name="newPrice"></param>
        /// <param name="oldMarketCap"></param>
        /// <param name="newMarketCap"></param>
        /// <param name="oldVolume"></param>
        /// <param name="newVolume"></param>
        /// <param name="updatedDate"></param>
        public ExchangePricePairUpdatedEvent(int id, PricingProviderType pricingProviderType, decimal oldPrice, decimal newPrice, decimal? oldMarketCap,
            decimal? newMarketCap, decimal? oldVolume, decimal? newVolume, DateTime updatedDate)
        {
            Id = id;
            PricingProviderType = pricingProviderType;
            OldPrice = oldPrice;
            NewPrice = newPrice;
            OldMarketCap = oldMarketCap;
            NewMarketCap = newMarketCap;    
            OldVolume = oldVolume;
            NewVolume = newVolume;
            UpdatedDate = updatedDate;
        }
    }
}
