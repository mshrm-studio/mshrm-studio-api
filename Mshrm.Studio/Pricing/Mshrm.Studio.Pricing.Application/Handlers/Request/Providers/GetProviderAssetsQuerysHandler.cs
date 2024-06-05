using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairs.Queries;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Application.Services.Http.HttpService;
using Mshrm.Studio.Pricing.Application.Services.Providers;
using Mshrm.Studio.Pricing.Domain.ExchangePricingPairs.Queries;
using OpenTracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.Providers
{
    public class GetProviderAssetsQuerysHandler : IRequestHandler<GetProviderAssetsQuery, List<string>>
    {
        private readonly AssetPriceServiceResolver _assetPriceServiceResolver;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetProviderPricesQuerysHandler"/> class.
        /// </summary>
        /// <param name="exchangePricingPairRepository"></param>
        /// <param name="assetRepository"></param>
        public GetProviderAssetsQuerysHandler(AssetPriceServiceResolver assetPriceServiceResolver, ITracer tracer)
        {
            _assetPriceServiceResolver = assetPriceServiceResolver;

            _tracer = tracer;
        }

        /// <summary>
        /// Get the latest prices for symbols
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>Latest prices</returns>
        public async Task<List<string>> Handle(GetProviderAssetsQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("GetProviderAssetsAsync_GetProviderAssetsQuerysHandler").StartActive(true))
            {
                // Get provider
                var provider = _assetPriceServiceResolver(query.ProviderType);

                // Get assset
                var assets = await provider.GetAssetsAsync();

                // Return symbols
                return assets.Select(x => x.Symbol).ToList();
            }
        }
    }
}
