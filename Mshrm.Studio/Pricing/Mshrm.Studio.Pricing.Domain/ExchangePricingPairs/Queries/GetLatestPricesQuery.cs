using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairs.Queries
{
    public class GetLatestPricesQuery : IRequest<List<ExchangePricingPair>>
    {
        public List<string>? Symbols { get; set; }
        public string BaseCurrencySymbol { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        public PricingProviderType? PricingProviderType { get; set; }
    }
}
