using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Models.CQRS.Currencies.Queries
{
    public class GetCurrenciesQuery : IRequest<List<Currency>>
    {
        public PricingProviderType PricingProviderType { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        public List<string>? Symbols { get; set; }
        public bool? Active { get; set; }
    }
}
