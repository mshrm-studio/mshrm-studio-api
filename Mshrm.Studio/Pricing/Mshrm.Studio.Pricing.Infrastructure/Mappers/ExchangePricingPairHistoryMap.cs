using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Models.Maps
{
    public class ExchangePricingPairHistoryMap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityTypeBuilder">Builder</param>
        public ExchangePricingPairHistoryMap(EntityTypeBuilder<ExchangePricingPairHistory> entityTypeBuilder)
        {
            // Define table mapping
            entityTypeBuilder.ToTable("ExchangePricingPairHistories", "dbo");

            // Id
            entityTypeBuilder.HasKey(t => t.Id);
            entityTypeBuilder.Property(t => t.Id)
                .UseIdentityColumn(1, 1);

            // Guid Id
            entityTypeBuilder.Property(t => t.GuidId)
                .IsRequired()
                .HasValueGenerator<GuidValueGenerator>();

            entityTypeBuilder.HasOne(x => x.ExchangePricingPair).WithMany(x => x.ExchangePricingPairHistories).HasForeignKey(x => x.ExchangePricingPairId);

            entityTypeBuilder.Property(e => e.PricingProviderType).HasConversion(new EnumToStringConverter<PricingProviderType>());
        }
    }
}
