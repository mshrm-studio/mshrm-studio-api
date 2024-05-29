using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Mshrm.Studio.Domain.Api.Models.Entity;

namespace Mshrm.Studio.Domain.Api.Models.EntityMaps
{
    /// <summary>
    /// Mapping class for a user
    /// </summary>
    public class UserMap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityTypeBuilder">Builder</param>
        public UserMap(EntityTypeBuilder<User> entityTypeBuilder)
        {
            // Define table mapping
            entityTypeBuilder.ToTable("Users", "dbo");

            // Id
            entityTypeBuilder.HasKey(t => t.Id);
            entityTypeBuilder.Property(t => t.Id)
                .UseIdentityColumn(1, 1);

            // Guid Id
            entityTypeBuilder.Property(t => t.GuidId)
                .IsRequired()
                .HasValueGenerator<GuidValueGenerator>();
        }
    }
}
