using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Models.Events
{
    public class ExchangePricePairCreatedEvent : INotification
    {
        public ExchangePricingPair ExchangePricingPair { get; }

        public ExchangePricePairCreatedEvent(ExchangePricingPair exchangePricingPair)
        {
            ExchangePricingPair = exchangePricingPair;
        }
    }
}
