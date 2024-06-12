using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Domain.AssetPriceHistories;

namespace Mshrm.Studio.Pricing.Api.Models.Maps
{
    public class AssetPriceHistoryMap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityTypeBuilder">Builder</param>
        public AssetPriceHistoryMap(EntityTypeBuilder<AssetPriceHistory> entityTypeBuilder)
        {
            // Define table mapping
            entityTypeBuilder.ToTable("AssetPriceHistories", "dbo");

            // Id
            entityTypeBuilder.HasKey(t => t.Id);
            entityTypeBuilder.Property(t => t.Id)
                .UseIdentityColumn(1, 1);

            // Guid Id
            entityTypeBuilder.Property(t => t.GuidId)
                .IsRequired()
                .HasValueGenerator<GuidValueGenerator>();

            entityTypeBuilder.HasOne(x => x.AssetPrice).WithMany(x => x.AssetPriceHistories).HasForeignKey(x => x.AssetPriceId);

            entityTypeBuilder.Property(e => e.PricingProviderType).HasConversion(new EnumToStringConverter<PricingProviderType>());
        }
    }
}
