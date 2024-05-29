using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Shared.Interfaces;
using Mshrm.Studio.Shared.Models.Entities;
using Mshrm.Studio.Shared.Models.Entities.Bases;
using Mshrm.Studio.Shared.Models.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mshrm.Studio.Pricing.Api.Models.Entites
{ 
    [Index("GuidId")]
    public class ExchangePricingPairHistory : AuditableEntity, IAggregateRoot
    {
        public int Id { get; private set; }
        public Guid GuidId { get; private set; }
        public int ExchangePricingPairId { get; private set; }

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

        public ExchangePricingPair ExchangePricingPair { get; private set; }

        public ExchangePricingPairHistory(int exchangePricingPairId, PricingProviderType pricingProviderType, decimal oldPrice, decimal newPrice, 
            decimal? oldMarketCap, decimal? newMarketCap, decimal? oldVolume, decimal? newVolume)
        {
            OldPrice = oldPrice;
            NewPrice = newPrice;
            OldMarketCap = oldMarketCap;
            NewMarketCap = newMarketCap;
            OldVolume = oldVolume;
            NewVolume = newVolume;
            PricingProviderType = pricingProviderType;
            ExchangePricingPairId = exchangePricingPairId;

            CreatedDate = DateTime.UtcNow;
        }
    }
}
