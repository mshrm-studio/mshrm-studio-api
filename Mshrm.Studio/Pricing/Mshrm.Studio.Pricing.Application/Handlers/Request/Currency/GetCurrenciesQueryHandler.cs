using Mshrm.Studio.Pricing.Api.Models.CQRS.Currencies.Queries;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Models.Pagination;
using OpenTracing;
using System.Threading;
using MediatR;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Currencies.Commands;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.Currencies
{
    public class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, List<Currency>>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ITracer _tracer;

        public GetCurrenciesQueryHandler(ICurrencyRepository currencyRepository, ITracer tracer)
        {
            _currencyRepository = currencyRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Get a list of currencies
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A list of filtered currencies</returns>
        public async Task<List<Currency>> Handle(GetCurrenciesQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetCurrenciesReadOnlyAsync_CreateCurrencyService").StartActive(true))
            {
                return await _currencyRepository.GetCurrenciesReadOnlyAsync(query.CurrencyType, query.PricingProviderType, query.Active, query.Symbols);
            }
        }
    }
}
