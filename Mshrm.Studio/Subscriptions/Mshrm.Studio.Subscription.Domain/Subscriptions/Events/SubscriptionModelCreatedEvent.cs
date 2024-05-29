using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Subscription.Domain.Subscriptions.Events
{
    public class SubscriptionModelCreatedEvent : INotification
    {
        public SubscriptionModel SubscriptionModel { get; private set; }

        public SubscriptionModelCreatedEvent(SubscriptionModel subscriptionModel)
        {
            SubscriptionModel = subscriptionModel;
        }
    }
}
