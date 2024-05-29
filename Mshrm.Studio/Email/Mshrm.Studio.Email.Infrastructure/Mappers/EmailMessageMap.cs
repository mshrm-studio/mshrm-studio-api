using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Mshrm.Studio.Email.Api.Models.Entities;
using Mshrm.Studio.Shared.Enums;

namespace Mshrm.Studio.Email.Infrastructure.Mappers
{
    public class EmailMessageMap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityTypeBuilder">Builder</param>
        public EmailMessageMap(EntityTypeBuilder<EmailMessage> entityTypeBuilder)
        {
            // Define table mapping
            entityTypeBuilder.ToTable("EmailMessages", "dbo");

            // Id
            entityTypeBuilder.HasKey(t => t.Id);
            entityTypeBuilder.Property(t => t.Id)
                .UseIdentityColumn(1, 1);

            // Guid Id
            entityTypeBuilder.Property(t => t.GuidId)
                .IsRequired()
                .HasValueGenerator<GuidValueGenerator>();

            entityTypeBuilder.Property(e => e.EmailType).HasConversion(new EnumToStringConverter<EmailType>());
        }
    }
}
