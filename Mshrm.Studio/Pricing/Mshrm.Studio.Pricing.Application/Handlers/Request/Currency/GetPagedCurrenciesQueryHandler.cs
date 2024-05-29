using Mshrm.Studio.Pricing.Api.Models.CQRS.Currencies.Queries;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Models.Pagination;
using OpenTracing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Threading;
using MediatR;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.Currencies
{
    public class GetPagedCurrenciesQueryHandler : IRequestHandler<GetPagedCurrenciesQuery, PagedResult<Currency>>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ITracer _tracer;

        public GetPagedCurrenciesQueryHandler(ICurrencyRepository currencyRepository, ITracer tracer)
        {
            _currencyRepository = currencyRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Get currencies
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>Currencies</returns>
        public async Task<PagedResult<Currency>> Handle(GetPagedCurrenciesQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("CreateCurrencyAsync_CreateCurrencyService").StartActive(true))
            {
                return await _currencyRepository.GetCurrenciesPagedAsync(query.Search, query.Symbol, query.Name, query.PricingProviderType, query.CurrencyType,
                    new Page(query.PageNumber, query.PerPage), new SortOrder(query.OrderProperty, query.Order), cancellationToken);
            }
        }
    }
}
