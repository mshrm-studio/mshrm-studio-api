using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Mshrm.Studio.Storage.Api.Models.Entities;
using Mshrm.Studio.Storage.Api.Models.Enums;

namespace Mshrm.Studio.Storage.Api.Models.Maps
{
    public class ResourceMap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityTypeBuilder">Builder</param>
        public ResourceMap(EntityTypeBuilder<Resource> entityTypeBuilder)
        {
            // Define table mapping
            entityTypeBuilder.ToTable("Resources", "dbo");

            // Id
            entityTypeBuilder.HasKey(t => t.Id);
            entityTypeBuilder.Property(t => t.Id)
                .UseIdentityColumn(1, 1);

            // Guid Id
            entityTypeBuilder.Property(t => t.GuidId)
                .IsRequired()
                .HasValueGenerator<GuidValueGenerator>();

            entityTypeBuilder.Property(x => x.IsPrivate).HasDefaultValue(false);

            entityTypeBuilder.Property(e => e.AssetType).HasConversion(new EnumToStringConverter<AssetType>());
        }
    }
}
