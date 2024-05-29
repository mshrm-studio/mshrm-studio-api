using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Models.Entities.Bases;
using Mshrm.Studio.Shared.Models.Entities.Interfaces;
using Mshrm.Studio.Subscription.Domain.PricingStructures;
using Mshrm.Studio.Subscription.Domain.PricingStructures.Events;
using Mshrm.Studio.Subscription.Domain.Subscriptions.Enums;
using Mshrm.Studio.Subscription.Domain.Subscriptions.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Subscription.Domain.Subscriptions
{
    public class SubscriptionModel : AuditableEntity, IAggregateRoot
    {
        public int Id { get; private set; }
        public Guid GuidId { get; private set; }
        public SubscriptionModelType SubscriptionModelType { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; } 
        public Guid? LogoGuidId { get; private set; }
        public bool Active { get; private set; }

        public List<PricingStructure> PricingStructures { get; private set; } = new();

        public SubscriptionModel(SubscriptionModelType subscriptionModelType, string name, string? description, Guid? logoGuidId, bool active)
        {
            SubscriptionModelType = subscriptionModelType;
            Name = name;
            Description = description;
            LogoGuidId = logoGuidId;
            Active = active;

            base.QueueDomainEvent(new SubscriptionModelCreatedEvent(this));
        }

        public void AddPricingStructure(PricingStructure pricingStructure)
        {
            PricingStructures.Add(pricingStructure);

            base.QueueDomainEvent(new PricingStructureAddedEvent(Id, pricingStructure));
        }

        public void RemovePricingStructure(Guid guidId)
        {
            // Get pricing structure
            var pricingStructure = PricingStructures.Where(p => p.GuidId == guidId).FirstOrDefault();
            if (pricingStructure == null)
            {
                throw new NotFoundException("Pricing structure doesn't exist", FailureCode.PricingStructureDoesntExist);
            }

            // Remove
            PricingStructures.Remove(pricingStructure);

            // Fire event
            base.QueueDomainEvent(new PricingStructureRemovedEvent(Id, pricingStructure));
        }

        public void UpdateLogo(Guid? logoGuidId)
        {
            LogoGuidId = logoGuidId;
        }

        public void UpdateInfo(string name, string? description)
        {
            Name = name;
            Description = description;  
        }

        public void Deactivate()
        {
            Active = false;

            // Fire event
            base.QueueDomainEvent(new SubscriptionDeactivatedEvent(this));
        }

        public void Activate()
        {
            Active = true;

            // Fire event
            base.QueueDomainEvent(new SubscriptionActivatedEvent(this));
        }
    }
}
