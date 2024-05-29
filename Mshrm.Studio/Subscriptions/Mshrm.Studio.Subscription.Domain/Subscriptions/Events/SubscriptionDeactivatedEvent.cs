using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Subscription.Domain.Subscriptions.Events
{
    public class SubscriptionDeactivatedEvent : INotification
    {
        public SubscriptionModel SubscriptionModel { get; private set; }

        public SubscriptionDeactivatedEvent(SubscriptionModel subscriptionModel)
        {
            SubscriptionModel = subscriptionModel;
        }
    }
}
