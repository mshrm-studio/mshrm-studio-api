using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Models.Maps
{
    public class AssetPriceMap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityTypeBuilder">Builder</param>
        public AssetPriceMap(EntityTypeBuilder<AssetPrice> entityTypeBuilder)
        {
            // Define table mapping
            entityTypeBuilder.ToTable("AssetPrices", "dbo");

            // Id
            entityTypeBuilder.HasKey(t => t.Id);
            entityTypeBuilder.Property(t => t.Id)
                .UseIdentityColumn(1, 1);

            // Guid Id
            entityTypeBuilder.Property(t => t.GuidId)
                .IsRequired()
                .HasValueGenerator<GuidValueGenerator>();

            entityTypeBuilder.HasOne(x => x.BaseAsset).WithMany(x => x.BaseAssetPrices).HasForeignKey(x => x.BaseAssetId).OnDelete(DeleteBehavior.Restrict); ;
            entityTypeBuilder.HasOne(x => x.Asset).WithMany(x => x.AssetPrices).HasForeignKey(x => x.AssetId).OnDelete(DeleteBehavior.Restrict);
            entityTypeBuilder.HasMany(x => x.AssetPriceHistories).WithOne(x => x.AssetPrice).HasForeignKey(x => x.AssetPriceId);

            entityTypeBuilder.Property(e => e.PricingProviderType).HasConversion(new EnumToStringConverter<PricingProviderType>());
        }
    }
}
