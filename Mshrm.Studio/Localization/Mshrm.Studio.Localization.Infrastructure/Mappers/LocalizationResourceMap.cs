using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Mshrm.Studio.Localization.Api.Models.Entities;
using Mshrm.Studio.Localization.Api.Models.Enums;

namespace Mshrm.Studio.Localization.Api.Models.Maps
{
    public class LocalizationResourceMap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityTypeBuilder">Builder</param>
        public LocalizationResourceMap(EntityTypeBuilder<LocalizationResource> entityTypeBuilder)
        {
            // Define table mapping
            entityTypeBuilder.ToTable("LocalizationResources", "dbo");

            // Id
            entityTypeBuilder.HasKey(t => t.Id);
            entityTypeBuilder.Property(t => t.Id)
                .UseIdentityColumn(1, 1);

            // Guid Id
            entityTypeBuilder.Property(t => t.GuidId)
                .IsRequired()
                .HasValueGenerator<GuidValueGenerator>();

            entityTypeBuilder.Property(e => e.LocalizationArea).HasConversion(new EnumToStringConverter<LocalizationArea>());
        }
    }
}
