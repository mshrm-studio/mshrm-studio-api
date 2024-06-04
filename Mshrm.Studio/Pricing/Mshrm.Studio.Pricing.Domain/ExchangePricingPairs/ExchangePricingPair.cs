using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Models.Events;
using Mshrm.Studio.Shared.Interfaces;
using Mshrm.Studio.Shared.Models.Entities;
using Mshrm.Studio.Shared.Models.Entities.Bases;
using Mshrm.Studio.Shared.Models.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mshrm.Studio.Pricing.Api.Models.Entites
{ 
    [Index("GuidId")]
    public class ExchangePricingPair : AuditableEntity, IAggregateRoot
    {
        public int Id { get; private set; }
        public Guid GuidId { get; private set; }
        public int BaseAssetId { get; private set; }
        public int AssetId { get; private set; }

        [Column(TypeName = "decimal(28, 8)")]
        public decimal Price { get; private set; }

        [Column(TypeName = "decimal(28, 8)")]
        public decimal? MarketCap { get; private set; }

        [Column(TypeName = "decimal(28, 8)")]
        public decimal? Volume { get; private set; }

        public PricingProviderType PricingProviderType { get; private set; }

        public Asset BaseAsset { get; private set; }
        public Asset Asset { get; private set; }
        public List<ExchangePricingPairHistory> ExchangePricingPairHistories { get; private set; }

        public ExchangePricingPair(int baseAssetId, int assetId, decimal price, decimal? marketCap, decimal? volume, PricingProviderType pricingProviderType)
        {
            PricingProviderType = pricingProviderType;
            BaseAssetId = baseAssetId;
            AssetId = assetId;
            Price = price;
            CreatedDate = DateTime.UtcNow;
            MarketCap = marketCap;
            Volume = volume;

            base.QueueDomainEvent(new ExchangePricePairCreatedEvent(this));
        }

        public void UpdateData(decimal newPrice, decimal? newMarketCap, decimal? newVolume, PricingProviderType pricingProviderType)
        {
            base.QueueDomainEvent(new ExchangePricePairUpdatedEvent(Id, pricingProviderType, Price, newPrice, MarketCap, newMarketCap, Volume, newVolume, DateTime.UtcNow));

            PricingProviderType = pricingProviderType;
            Price = newPrice;
            MarketCap = newMarketCap;
            Volume = newVolume;
        }

        /// <summary>
        /// Do not call this before save - only for returning read only
        /// </summary>
        /// <param name="newBaseAsset">The base asset to set price as</param>
        /// <param name="oldBasePrice">The base asset price of the original base</param>
        /// <param name="newBasePrice">The base asset price of the new base</param>
        public void SetNewBaseAsset(Asset newBaseAsset, ExchangePricingPair oldBasePrice, ExchangePricingPair newBasePrice)
        {
            // Set the new base asset
            BaseAsset = newBaseAsset;
            BaseAssetId = newBaseAsset.Id;

            // Update the price
            Price = (oldBasePrice.Price / newBasePrice.Price) * Price;

            // Update the market cap
            if (MarketCap.HasValue)
            {
                MarketCap = (oldBasePrice.Price / newBasePrice.Price) * MarketCap;
            }
        }

        public void SetPrice(decimal price)
        {
            Price = price;
        }
    }
}
