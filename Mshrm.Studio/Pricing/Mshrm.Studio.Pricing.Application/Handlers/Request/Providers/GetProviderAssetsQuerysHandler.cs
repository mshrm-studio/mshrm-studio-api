using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairs.Queries;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Application.Services.Providers;
using Mshrm.Studio.Pricing.Domain.ProviderAssets;
using Mshrm.Studio.Pricing.Domain.ProviderAssets.Queries;
using Mshrm.Studio.Shared.Services.Interfaces;
using OpenTracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.Providers
{
    public class GetProviderAssetsQuerysHandler : IRequestHandler<GetProviderAssetsQuery, List<ProviderAsset>>
    {
        private readonly AssetPriceServiceResolver _assetPriceServiceResolver;
        private readonly ICacheService _cacheService;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetProviderPricesQuerysHandler"/> class.
        /// </summary>
        /// <param name="exchangePricingPairRepository"></param>
        /// <param name="assetRepository"></param>
        public GetProviderAssetsQuerysHandler(AssetPriceServiceResolver assetPriceServiceResolver, ICacheService cacheService, ITracer tracer)
        {
            _assetPriceServiceResolver = assetPriceServiceResolver;
            _cacheService = cacheService;
            _tracer = tracer;
        }

        /// <summary>
        /// Get the latest prices for symbols
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>Latest prices</returns>
        public async Task<List<ProviderAsset>> Handle(GetProviderAssetsQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetProviderAssetsAsync_GetProviderAssetsQuerysHandler").StartActive(true))
            {
                // Get provider
                var provider = _assetPriceServiceResolver(query.ProviderType);

                // Get assets
                var assets = await _cacheService.GetOrSetItemAsync<List<ProviderAsset>>(
                    $"Provider_{query.ProviderType}_assets", 
                    async () => await provider.GetAssetsAsync(),
                    cancellationToken,
                    44640
                );

                // Return symbols
                return assets;
            }
        }
    }
}
