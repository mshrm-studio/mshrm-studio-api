using MediatR;
using Microsoft.Extensions.Logging;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairHistories.Commands;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Application.Handlers.Request.ExchangePricePair;
using Mshrm.Studio.Pricing.Domain.ExchangePricingPairHistories.Queries;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Models.Pagination;
using OpenTracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.ExchangePricePairHistory
{
    public class GetPagedPriceHistoryQueryHandler : IRequestHandler<GetPagedPriceHistoryQuery, PagedResult<ExchangePricingPairHistory>>
    {
        private readonly IExchangePricingPairHistoryRepository _exchangePricingPairHistoryRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly ILogger<GetPagedPriceHistoryQuery> _logger;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPagedPriceHistoryQueryHandler"/> class.
        /// </summary>
        /// <param name="exchangePricingPairHistoryRepository"></param>
        /// <param name="assetRepository"></param>
        /// <param name="logger"></param>
        /// <param name="tracer"></param>
        public GetPagedPriceHistoryQueryHandler(IExchangePricingPairHistoryRepository exchangePricingPairHistoryRepository, IAssetRepository assetRepository,
            ILogger<GetPagedPriceHistoryQuery> logger, ITracer tracer)
        {
            _exchangePricingPairHistoryRepository = exchangePricingPairHistoryRepository;
            _assetRepository = assetRepository;

            _tracer = tracer;
            _logger = logger;
        }

        /// <summary>
        /// Get price history
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The prices added/updated</returns>
        public async Task<PagedResult<ExchangePricingPairHistory>> Handle(GetPagedPriceHistoryQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("AddHistoryAsync_CreatePriceHistoryService").StartActive(true))
            {
                // Get the asset the history is for
                var asset = await _assetRepository.GetAssetAsync(Guid.Parse(query.AssetGuidId), true, cancellationToken);
                if (asset == null)
                {
                    throw new NotFoundException("Asset doesn't exist", Shared.Enums.FailureCode.AssetDoesntExist);
                }

                // Get the asset to convert price in history for
                var baseAsset = await _assetRepository.GetAssetAsync(Guid.Parse(query.BaseAssetGuidId), true, cancellationToken);
                if(baseAsset == null)
                {
                    throw new NotFoundException("Asset to convert price to doesn't exist", Shared.Enums.FailureCode.AssetDoesntExist);
                }

                var pagedHistory = await _exchangePricingPairHistoryRepository.GetPagedHistory(asset.Id, query.PricingProviderType,
                    new Page(query.PageNumber, query.PerPage), new SortOrder(query.OrderProperty, query.Order), cancellationToken);

                // Convert the price
                //TODO:

                return pagedHistory;
            }
        }
    }
}
