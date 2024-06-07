using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Models.Maps
{
    public class AssetMap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityTypeBuilder">Builder</param>
        public AssetMap(EntityTypeBuilder<Asset> entityTypeBuilder)
        {
            // Define table mapping
            entityTypeBuilder.ToTable("Assets", "dbo");

            // Id
            entityTypeBuilder.HasKey(t => t.Id);
            entityTypeBuilder.Property(t => t.Id)
                .UseIdentityColumn(1, 1);

            // Guid Id
            entityTypeBuilder.Property(t => t.GuidId)
                .IsRequired()
                .HasValueGenerator<GuidValueGenerator>();

            entityTypeBuilder.HasMany(x => x.BasePricingPairs).WithOne(x => x.BaseAsset).HasForeignKey(x => x.BaseAssetId);
            entityTypeBuilder.HasMany(x => x.PricingPairs).WithOne(x => x.Asset).HasForeignKey(x => x.AssetId);

            entityTypeBuilder.Property(e => e.ProviderType).HasConversion(new EnumToStringConverter<PricingProviderType>());
            entityTypeBuilder.Property(e => e.AssetType).HasConversion(new EnumToStringConverter<AssetType>());

            entityTypeBuilder.Property(e => e.DecimalPlaces).HasDefaultValue(3);
        }
    }
}
