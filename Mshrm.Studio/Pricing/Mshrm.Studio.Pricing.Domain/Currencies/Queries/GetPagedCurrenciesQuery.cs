using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Pricing.Api.Models.CQRS.Currencies.Queries
{
    public class GetPagedCurrenciesQuery : IRequest<PagedResult<Currency>>
    {
        public string? Search { get; set; }
        public string? Symbol { get; set; }
        public string? Name { get; set; }
        public PricingProviderType? PricingProviderType { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        public string OrderProperty { get; set; }
        public Order Order { get; set; }
        public uint PageNumber { get; set; }
        public uint PerPage { get; set; }
    }
}
