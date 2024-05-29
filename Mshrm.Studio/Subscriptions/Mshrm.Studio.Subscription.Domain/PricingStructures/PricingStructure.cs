using Mshrm.Studio.Shared.Models.Entities.Bases;
using Mshrm.Studio.Shared.Models.Entities.Interfaces;
using Mshrm.Studio.Subscription.Domain.PricingStructures.Enums;
using Mshrm.Studio.Subscription.Domain.PricingStructures.ValueObjects;
using Mshrm.Studio.Subscription.Domain.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Subscription.Domain.PricingStructures
{
    public class PricingStructure : AuditableEntity
    {
        public int Id { get; private set; }
        public Guid GuidId { get; private set; }
        public PeriodType PeriodType { get; private set; }
        public Discount Discount { get; private set; }
        public string SupportedCurrency { get; private set; }
        public decimal Price { get; private set; }
        public int Rank { get; private set; }
        public DateTime? ExpiresDate { get; private set; }
        public int SubcriptionModelId { get; private set; }

        public SubscriptionModel SubcriptionModel { get; private set; }

        public PricingStructure() 
        {

        }
    }
}
