using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairs.Commands
{
    public class ImportPricesCommand : IRequest<bool>
    {
        public PricingProviderType PricingProviderType { get; set; }
    }
}
