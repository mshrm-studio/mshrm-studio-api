using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Models.Enums;

namespace Mshrm.Studio.Domain.Api.Models.EntityMaps
{
    /// <summary>
    /// Mapping class for a user
    /// </summary>
    public class ToolMap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityTypeBuilder">Builder</param>
        public ToolMap(EntityTypeBuilder<Tool> entityTypeBuilder)
        {
            // Define table mapping
            entityTypeBuilder.ToTable("Tools", "dbo");

            // Id
            entityTypeBuilder.HasKey(t => t.Id);
            entityTypeBuilder.Property(t => t.Id)
                .UseIdentityColumn(1, 1);

            // Guid Id
            entityTypeBuilder.Property(t => t.GuidId)
                .IsRequired()
                .HasValueGenerator<GuidValueGenerator>();

            entityTypeBuilder.Property(e => e.ToolType).HasConversion(new EnumToStringConverter<ToolType>());
        }
    }
}
