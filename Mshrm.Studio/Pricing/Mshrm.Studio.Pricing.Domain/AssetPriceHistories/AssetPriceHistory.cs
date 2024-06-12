using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Shared.Interfaces;
using Mshrm.Studio.Shared.Models.Entities;
using Mshrm.Studio.Shared.Models.Entities.Bases;
using Mshrm.Studio.Shared.Models.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mshrm.Studio.Pricing.Domain.AssetPriceHistories
{ 
    [Index("GuidId")]
    public class AssetPriceHistory : AuditableEntity, IAggregateRoot
    {
        public int Id { get; private set; }
        public Guid GuidId { get; private set; }
        public int AssetPriceId { get; private set; }

        [Column(TypeName = "decimal(28, 8)")]
        public decimal OldPrice { get; private set; }

        [Column(TypeName = "decimal(28, 8)")]
        public decimal NewPrice { get; private set; }

        [Column(TypeName = "decimal(28, 8)")]
        public decimal? OldMarketCap { get; private set; }

        [Column(TypeName = "decimal(28, 8)")]
        public decimal? NewMarketCap { get; private set; }

        [Column(TypeName = "decimal(28, 8)")]
        public decimal? OldVolume { get; private set; }

        [Column(TypeName = "decimal(28, 8)")]
        public decimal? NewVolume { get; private set; }

        public PricingProviderType PricingProviderType { get; private set; }

        public AssetPrice AssetPrice { get; private set; }

        public AssetPriceHistory(int assetPriceId, PricingProviderType pricingProviderType, decimal oldPrice, decimal newPrice, 
            decimal? oldMarketCap, decimal? newMarketCap, decimal? oldVolume, decimal? newVolume)
        {
            OldPrice = oldPrice;
            NewPrice = newPrice;
            OldMarketCap = oldMarketCap;
            NewMarketCap = newMarketCap;
            OldVolume = oldVolume;
            NewVolume = newVolume;
            PricingProviderType = pricingProviderType;
            AssetPriceId = assetPriceId;

            CreatedDate = DateTime.UtcNow;
        }
    }
}
