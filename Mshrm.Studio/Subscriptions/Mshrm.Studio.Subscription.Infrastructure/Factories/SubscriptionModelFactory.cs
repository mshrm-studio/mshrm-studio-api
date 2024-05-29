using Mshrm.Studio.Subscription.Domain.Subscriptions;
using Mshrm.Studio.Subscription.Domain.Subscriptions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Subscription.Infrastructure.Factories
{
    public class SubscriptionModelFactory : ISubscriptionModelFactory
    {
        /// <summary>
        /// Create a new subscription model
        /// </summary>
        /// <param name="subscriptionModelType">The subscription model types</param>
        /// <param name="name">A name</param>
        /// <param name="description">A description</param>
        /// <param name="pricingStructureId">The pricing structure</param>
        /// <param name="logoGuidId">A image for the subscription</param>
        /// <returns>A new subscription model</returns>
        public SubscriptionModel CreateSubscriptionModel(SubscriptionModelType subscriptionModelType, string name, string? description, int? pricingStructureId, Guid? logoGuidId)
        {
            return new SubscriptionModel(subscriptionModelType, name, description, logoGuidId, true);
        }
    }
}
