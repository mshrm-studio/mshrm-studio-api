using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Models.Maps
{
    public class ExchangePricingPairMap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityTypeBuilder">Builder</param>
        public ExchangePricingPairMap(EntityTypeBuilder<ExchangePricingPair> entityTypeBuilder)
        {
            // Define table mapping
            entityTypeBuilder.ToTable("ExchangePricingPairs", "dbo");

            // Id
            entityTypeBuilder.HasKey(t => t.Id);
            entityTypeBuilder.Property(t => t.Id)
                .UseIdentityColumn(1, 1);

            // Guid Id
            entityTypeBuilder.Property(t => t.GuidId)
                .IsRequired()
                .HasValueGenerator<GuidValueGenerator>();

            entityTypeBuilder.HasOne(x => x.BaseCurrency).WithMany(x => x.BasePricingPairs).HasForeignKey(x => x.BaseCurrencyId).OnDelete(DeleteBehavior.Restrict); ;
            entityTypeBuilder.HasOne(x => x.Currency).WithMany(x => x.PricingPairs).HasForeignKey(x => x.CurrencyId).OnDelete(DeleteBehavior.Restrict);
            entityTypeBuilder.HasMany(x => x.ExchangePricingPairHistories).WithOne(x => x.ExchangePricingPair).HasForeignKey(x => x.ExchangePricingPairId);

            entityTypeBuilder.Property(e => e.PricingProviderType).HasConversion(new EnumToStringConverter<PricingProviderType>());
        }
    }
}
