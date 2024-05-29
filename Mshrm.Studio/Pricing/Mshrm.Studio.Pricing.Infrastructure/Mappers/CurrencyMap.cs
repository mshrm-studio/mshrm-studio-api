using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Models.Maps
{
    public class CurrencyMap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityTypeBuilder">Builder</param>
        public CurrencyMap(EntityTypeBuilder<Currency> entityTypeBuilder)
        {
            // Define table mapping
            entityTypeBuilder.ToTable("Currencies", "dbo");

            // Id
            entityTypeBuilder.HasKey(t => t.Id);
            entityTypeBuilder.Property(t => t.Id)
                .UseIdentityColumn(1, 1);

            // Guid Id
            entityTypeBuilder.Property(t => t.GuidId)
                .IsRequired()
                .HasValueGenerator<GuidValueGenerator>();

            entityTypeBuilder.HasMany(x => x.BasePricingPairs).WithOne(x => x.BaseCurrency).HasForeignKey(x => x.BaseCurrencyId);
            entityTypeBuilder.HasMany(x => x.PricingPairs).WithOne(x => x.Currency).HasForeignKey(x => x.CurrencyId);

            entityTypeBuilder.Property(e => e.ProviderType).HasConversion(new EnumToStringConverter<PricingProviderType>());
            entityTypeBuilder.Property(e => e.CurrencyType).HasConversion(new EnumToStringConverter<CurrencyType>());
        }
    }
}
