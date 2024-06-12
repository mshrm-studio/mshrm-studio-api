using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Models.Pagination;
using OpenTracing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Threading;
using MediatR;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Assets.Queries;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.Assets
{
    public class GetPagedAssetsQueryHandler : IRequestHandler<GetPagedAssetsQuery, PagedResult<Asset>>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ITracer _tracer;

        public GetPagedAssetsQueryHandler(IAssetRepository assetRepository, ITracer tracer)
        {
            _assetRepository = assetRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Get assets
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>Assets</returns>
        public async Task<PagedResult<Asset>> Handle(GetPagedAssetsQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("CreateAssetAsync_CreateAssetService").StartActive(true))
            {
                return await _assetRepository.GetAssetsPagedAsync(query.Search, query.Symbol, query.Name, query.PricingProviderType, query.AssetType,
                    new Page(query.PageNumber, query.PerPage), new SortOrder(query.OrderProperty, query.Order), cancellationToken);
            }
        }
    }
}
