using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Subscription.Domain.PricingStructures.Events
{
    public class PricingStructureAddedEvent : INotification
    {
        public int SubscriptionId { get; private set; }
        public PricingStructure PricingStructure { get; private set; }

        public PricingStructureAddedEvent(int subscriptionId, PricingStructure pricingStructure) 
        { 
            SubscriptionId = subscriptionId;
            PricingStructure = pricingStructure;
        }
    }
}
