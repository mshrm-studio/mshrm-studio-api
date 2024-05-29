using MediatR;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Currencies.Queries;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairs.Queries;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using OpenTracing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.ExchangePricePair
{
    public class GetLatestPricesQueryHandler : IRequestHandler<GetLatestPricesQuery, List<ExchangePricingPair>>
    {
        private readonly IExchangePricingPairRepository _exchangePricingPairRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestPricesQueryHandler"/> class.
        /// </summary>
        /// <param name="exchangePricingPairRepository"></param>
        /// <param name="currencyRepository"></param>
        public GetLatestPricesQueryHandler(IExchangePricingPairRepository exchangePricingPairRepository, ICurrencyRepository currencyRepository, ITracer tracer)
        {
            _exchangePricingPairRepository = exchangePricingPairRepository;
            _currencyRepository = currencyRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Get the latest prices for symbols
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>Latest prices</returns>
        public async Task<List<ExchangePricingPair>> Handle(GetLatestPricesQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("CreateCurrencyAsync_CreateCurrencyService").StartActive(true))
            {
                // Make sure everything provided is upper
                if (query.Symbols != null)
                    query.Symbols = query.Symbols.Select(x => x.Trim().ToUpper()).ToList();

                // Get base currency
                var baseCurrency = (await _currencyRepository.GetCurrenciesReadOnlyAsync(null, null, true, new List<string>() { query.BaseCurrencySymbol })).FirstOrDefault();
                if (baseCurrency == null)
                    throw new UnprocessableEntityException("The base currency symbol provided is not supported", FailureCode.BaseCurrencyNotSupported);

                // Get the OLD base currency price
                var oldBaseCurrency = (await _currencyRepository.GetCurrenciesReadOnlyAsync(null, null, true, new List<string>() { "USD" })).FirstOrDefault();
                var oldBaseCurrencyPrice = (await _exchangePricingPairRepository.GetLatestExchangePricingPairsReadOnlyAsync(new List<int>() { oldBaseCurrency.Id }, null, null, cancellationToken)).FirstOrDefault();

                // Get NEW base currency price
                var newBaseCurrencyPrice = (await _exchangePricingPairRepository.GetLatestExchangePricingPairsReadOnlyAsync(new List<int>() { baseCurrency.Id }, null, null, cancellationToken)).FirstOrDefault();
                if (newBaseCurrencyPrice == null)
                    throw new UnprocessableEntityException("The base currency symbols price doesn't exist", FailureCode.BaseCurrencyPriceDoesntExist);

                // Get currencies
                var currencies = await _currencyRepository.GetCurrenciesReadOnlyAsync(query.CurrencyType, query.PricingProviderType, true, query.Symbols);
                var currencyIds = currencies.Select(x => x.Id).ToList();

                // Get prices
                var prices = await _exchangePricingPairRepository.GetLatestExchangePricingPairsReadOnlyAsync(currencyIds, query.PricingProviderType, query.CurrencyType,
                    cancellationToken);

                // Convert to base currency price
                prices.ForEach(x => { x.SetNewBaseCurrency(baseCurrency, oldBaseCurrencyPrice, newBaseCurrencyPrice); });

                return prices;
            }
        }
    }
}
