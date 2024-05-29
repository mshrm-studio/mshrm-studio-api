using MediatR;
using Microsoft.Extensions.Logging;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairHistories.Commands;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairs.Commands;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Application.Handlers.Request.ExchangePricePair;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using OpenTracing;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.ExchangePricePairHistory
{
    /// <summary>
    /// Create a price pair
    /// </summary>
    public class CreateExchangePricingPairHistoryCommandHandler : IRequestHandler<CreateExchangePricingPairHistoryCommand, ExchangePricingPairHistory>
    {
        private readonly IExchangePricingPairHistoryRepository _exchangePricingPairHistoryRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ILogger<CreateOrReplacePricingPairsCommandHandler> _logger;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrReplacePricingPairsCommandHandler"/> class.
        /// </summary>
        /// <param name="exchangePricingPairHistoryRepository"></param>
        /// <param name="currencyRepository"></param>
        /// <param name="logger"></param>
        /// <param name="tracer"></param>
        public CreateExchangePricingPairHistoryCommandHandler(IExchangePricingPairHistoryRepository exchangePricingPairHistoryRepository, ICurrencyRepository currencyRepository, ILogger<CreateOrReplacePricingPairsCommandHandler> logger, ITracer tracer)
        {
            _exchangePricingPairHistoryRepository = exchangePricingPairHistoryRepository;
            _currencyRepository = currencyRepository;

            _tracer = tracer;
            _logger = logger;
        }

        /// <summary>
        /// Add a history item
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The prices added/updated</returns>
        public async Task<ExchangePricingPairHistory> Handle(CreateExchangePricingPairHistoryCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("AddHistoryAsync_CreatePriceHistoryService").StartActive(true))
            {
                return await _exchangePricingPairHistoryRepository.AddHistoryAsync(command.Id, command.OldPrice, command.NewPrice, command.OldMarketCap, command.NewMarketCap,
                    command.OldVolume, command.NewVolume, command.PricingProviderType, cancellationToken);
            }
        }
    }
}
