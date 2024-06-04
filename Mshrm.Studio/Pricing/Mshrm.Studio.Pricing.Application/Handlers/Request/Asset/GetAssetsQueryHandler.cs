using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Models.Pagination;
using OpenTracing;
using System.Threading;
using MediatR;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Assets.Commands;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Assets.Queries;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.Assets
{
    public class GetAssetsQueryHandler : IRequestHandler<GetAssetsQuery, List<Asset>>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ITracer _tracer;

        public GetAssetsQueryHandler(IAssetRepository assetRepository, ITracer tracer)
        {
            _assetRepository = assetRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Get a list of assets
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A list of filtered assets</returns>
        public async Task<List<Asset>> Handle(GetAssetsQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetAssetsReadOnlyAsync_CreateAssetService").StartActive(true))
            {
                return await _assetRepository.GetAssetsReadOnlyAsync(query.AssetType, query.PricingProviderType, query.Active, query.Symbols);
            }
        }
    }
}
