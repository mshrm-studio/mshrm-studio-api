using MediatR;
using Microsoft.Extensions.Logging;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairHistories.Commands;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairs.Commands;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Domain.ExchangePricingPairs;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using OpenTracing;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.ExchangePricePair
{
    /// <summary>
    /// Create a price pair
    /// </summary>
    public class CreateOrReplacePricingPairsCommandHandler : IRequestHandler<CreateOrReplacePricingPairsCommand, List<ExchangePricingPair>>
    {
        private readonly IExchangePricingPairRepository _exchangePricingPairRepository;
        private readonly IExchangePricingPairFactory _exchangePricingPairFactory;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ILogger<CreateOrReplacePricingPairsCommandHandler> _logger;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrReplacePricingPairsCommandHandler"/> class.
        /// </summary>
        /// <param name="exchangePricingPairRepository"></param>
        /// <param name="currencyRepository"></param>
        /// <param name="logger"></param>
        /// <param name="tracer"></param>
        public CreateOrReplacePricingPairsCommandHandler(IExchangePricingPairRepository exchangePricingPairRepository, ICurrencyRepository currencyRepository, IExchangePricingPairFactory exchangePricingPairFactory,
            ILogger<CreateOrReplacePricingPairsCommandHandler> logger, ITracer tracer)
        {
            _exchangePricingPairFactory = exchangePricingPairFactory;
            _exchangePricingPairRepository = exchangePricingPairRepository;
            _currencyRepository = currencyRepository;

            _tracer = tracer;
            _logger = logger;
        }

        /// <summary>
        /// Create or replace existing pricing pairs in database
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The prices added/updated</returns>
        public async Task<List<ExchangePricingPair>> Handle(CreateOrReplacePricingPairsCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("CreateOrReplaceNewRatePairsAsync_CreatePriceService").StartActive(true))
            {
                // Get all the currencies for the symbols provided
                var allSymbols = command.Prices.Select(x => x.Currency).ToList();
                var allBaseSymbols = command.Prices.Select(x => x.BaseCurrency).ToList();
                var allCurrencies = await _currencyRepository.GetCurrenciesReadOnlyAsync(null, null, true, allSymbols);
                var allCurrencyIds = allCurrencies.Select(x => x.Id).ToList();
                var baseCurrencies = await _currencyRepository.GetCurrenciesReadOnlyAsync(null, null, true, allBaseSymbols);

                var existingPrices = await _exchangePricingPairRepository.GetLatestExchangePricingPairsReadOnlyAsync(allCurrencyIds, null, null, cancellationToken);

                var allPairs = new List<ExchangePricingPair>();
                var allPairsToAdd = new List<ExchangePricingPair>();
                var allPairsToUpdate = new List<ExchangePricingPair>();

                foreach (var price in command.Prices)
                {
                    // Check the price doesn't already exist, if so then its replaced
                    var existingPrice = existingPrices.FirstOrDefault(x => x.Currency.Symbol == price.Currency);
                    if (existingPrice != null)
                    {
                        existingPrice.SetPrice(price.Price);
                        allPairsToUpdate.Add(existingPrice);
                    }
                    else
                    {
                        // Get the base currency
                        var baseCurrency = baseCurrencies.FirstOrDefault(x => x.Symbol == price.BaseCurrency);
                        if (baseCurrency == null)
                        {
                            _logger.LogCritical($"Base currency: {price.BaseCurrency} doesn't exist");

                            throw new NotFoundException("Base currency doesn't exist", FailureCode.CurrencyDoesntExist);
                        }

                        // Get the currency
                        var currency = allCurrencies.FirstOrDefault(x => x.Symbol == price.Currency);
                        if (currency == null)
                        {
                            _logger.LogCritical($"Currency: {price.Currency} doesn't exist");

                            throw new NotFoundException("Base currency doesn't exist", FailureCode.CurrencyDoesntExist);
                        }

                        allPairsToAdd.Add(_exchangePricingPairFactory.CreateExchangePricingPair(baseCurrency.Id, currency.Id, price.Price, price.MarketCap, price.Volume, command.PricingProviderType));
                    }
                }

                // Add any updated ones
                var updated = await _exchangePricingPairRepository.UpdateExchangePricingPairsAsync(allPairsToUpdate, command.PricingProviderType, cancellationToken);
                if (updated.Any())
                {
                    allPairs.AddRange(updated);
                }

                // Add any new ones
                var added = await _exchangePricingPairRepository.CreateExchangePricingPairsAsync(allPairsToAdd, cancellationToken);
                if (added.Any())
                {
                    allPairs.AddRange(added);
                }

                return allPairsToUpdate;
            }
        }
    }
}
