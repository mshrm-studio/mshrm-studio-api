using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Mshrm.Studio.Subscription.Domain.PricingStructures;
using Mshrm.Studio.Subscription.Domain.PricingStructures.Enums;
using Mshrm.Studio.Subscription.Domain.Subscriptions;
using Mshrm.Studio.Subscription.Domain.Subscriptions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Subscription.Infrastructure.Mappers
{
    public class PricingStructureMap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityTypeBuilder">Builder</param>
        public PricingStructureMap(EntityTypeBuilder<PricingStructure> entityTypeBuilder)
        {
            // Define table mapping
            entityTypeBuilder.ToTable("PricingStructures", "dbo");

            // Id
            entityTypeBuilder.HasKey(t => t.Id);
            entityTypeBuilder.Property(t => t.Id)
                .UseIdentityColumn(1, 1);

            // Guid Id
            entityTypeBuilder.Property(t => t.GuidId)
                .IsRequired()
                .HasValueGenerator<GuidValueGenerator>();

            entityTypeBuilder.Property(e => e.PeriodType).HasConversion(new EnumToStringConverter<PeriodType>());

            entityTypeBuilder.HasOne(x => x.SubcriptionModel).WithMany(x => x.PricingStructures);

            entityTypeBuilder.OwnsOne(p => p.Discount).Property(p => p.Percent).HasColumnName("DiscountPercent");
            entityTypeBuilder.OwnsOne(p => p.Discount).Property(p => p.Amount).HasColumnName("DiscountAmount");
            entityTypeBuilder.OwnsOne(p => p.Discount).Property(p => p.Reason).HasColumnName("DiscountReason");
        }
    }
}
