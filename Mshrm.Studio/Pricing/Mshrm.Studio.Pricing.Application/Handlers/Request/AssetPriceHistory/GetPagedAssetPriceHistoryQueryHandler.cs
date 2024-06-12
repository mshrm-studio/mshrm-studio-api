using MediatR;
using Microsoft.Extensions.Logging;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Application.Handlers.Request.AssetPrices;
using Mshrm.Studio.Pricing.Domain.AssetPriceHistories;
using Mshrm.Studio.Pricing.Domain.AssetPriceHistories.Queries;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Models.Pagination;
using OpenTracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.AssetPriceHistories
{
    public class GetPagedAssetPriceHistoryQueryHandler : IRequestHandler<GetPagedAssetPriceHistoryQuery, PagedResult<AssetPriceHistory>>
    {
        private readonly IAssetPriceHistoryRepository _assetPriceHistoryRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly ILogger<GetPagedAssetPriceHistoryQuery> _logger;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPagedAssetPriceHistoryQueryHandler"/> class.
        /// </summary>
        /// <param name="assetPriceHistoryRepository"></param>
        /// <param name="assetRepository"></param>
        /// <param name="logger"></param>
        /// <param name="tracer"></param>
        public GetPagedAssetPriceHistoryQueryHandler(IAssetPriceHistoryRepository assetPriceHistoryRepository, IAssetRepository assetRepository,
            ILogger<GetPagedAssetPriceHistoryQuery> logger, ITracer tracer)
        {
            _assetPriceHistoryRepository = assetPriceHistoryRepository;
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
        public async Task<PagedResult<AssetPriceHistory>> Handle(GetPagedAssetPriceHistoryQuery query, CancellationToken cancellationToken)
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

                var pagedHistory = await _assetPriceHistoryRepository.GetPagedHistory(asset.Id, query.PricingProviderType,
                    new Page(query.PageNumber, query.PerPage), new SortOrder(query.OrderProperty, query.Order), cancellationToken);

                // Convert the price
                //TODO:

                return pagedHistory;
            }
        }
    }
}
