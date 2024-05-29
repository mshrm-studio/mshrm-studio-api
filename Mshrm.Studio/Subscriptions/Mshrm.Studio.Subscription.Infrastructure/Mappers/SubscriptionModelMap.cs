using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Mshrm.Studio.Subscription.Domain.Subscriptions;
using Mshrm.Studio.Subscription.Domain.Subscriptions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Subscription.Infrastructure.Mappers
{
    public class SubscriptionModelMap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityTypeBuilder">Builder</param>
        public SubscriptionModelMap(EntityTypeBuilder<SubscriptionModel> entityTypeBuilder)
        {
            // Define table mapping
            entityTypeBuilder.ToTable("SubscriptionModels", "dbo");

            // Id
            entityTypeBuilder.HasKey(t => t.Id);
            entityTypeBuilder.Property(t => t.Id)
                .UseIdentityColumn(1, 1);

            // Guid Id
            entityTypeBuilder.Property(t => t.GuidId)
                .IsRequired()
                .HasValueGenerator<GuidValueGenerator>();

            entityTypeBuilder.Property(e => e.SubscriptionModelType).HasConversion(new EnumToStringConverter<SubscriptionModelType>());
        }
    }
}
