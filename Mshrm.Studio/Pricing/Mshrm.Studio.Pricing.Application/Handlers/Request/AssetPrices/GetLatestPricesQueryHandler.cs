using MediatR;
using Mshrm.Studio.Pricing.Api.Models.CQRS.AssetPrices.Queries;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using OpenTracing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.AssetPrices
{
    public class GetLatestPricesQueryHandler : IRequestHandler<GetLatestPricesQuery, List<AssetPrice>>
    {
        private readonly IAssetPriceRepository _assetPriceRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestPricesQueryHandler"/> class.
        /// </summary>
        /// <param name="assetPriceRepository"></param>
        /// <param name="assetRepository"></param>
        public GetLatestPricesQueryHandler(IAssetPriceRepository assetPriceRepository, IAssetRepository assetRepository, ITracer tracer)
        {
            _assetPriceRepository = assetPriceRepository;
            _assetRepository = assetRepository;

            _tracer = tracer;
        }

        /// <summary>
        /// Get the latest prices for symbols
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>Latest prices</returns>
        public async Task<List<AssetPrice>> Handle(GetLatestPricesQuery query, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("CreateAssetAsync_CreateAssetService").StartActive(true))
            {
                // Make sure everything provided is upper
                if (query.Symbols != null)
                    query.Symbols = query.Symbols.Select(x => x.Trim().ToUpper()).ToList();

                // Get base asset
                var baseAsset = (await _assetRepository.GetAssetsReadOnlyAsync(null, null, true, new List<string>() { query.BaseAssetSymbol })).FirstOrDefault();
                if (baseAsset == null)
                    throw new UnprocessableEntityException("The asset symbol to return price in provided is not supported anymore", FailureCode.BaseAssetNotSupported);

                // Check is currency
                if (baseAsset.AssetType != AssetType.Fiat && baseAsset.AssetType != AssetType.Crypto)
                {
                    throw new UnprocessableEntityException("The asset to return price in must be of asset type fiat or crypto", FailureCode.AssetPriceMustBeACurrency);
                }

                // Get the OLD base asset price
                var oldBaseAsset = (await _assetRepository.GetAssetsReadOnlyAsync(null, null, true, new List<string>() { "USD" })).FirstOrDefault();
                var oldBaseAssetPrice = (await _assetPriceRepository.GetLatestAssetPricesReadOnlyAsync(new List<int>() { oldBaseAsset.Id }, null, null, cancellationToken)).FirstOrDefault();

                // Get NEW base asset price
                var newBaseAssetPrice = (await _assetPriceRepository.GetLatestAssetPricesReadOnlyAsync(new List<int>() { baseAsset.Id }, null, null, cancellationToken)).FirstOrDefault();
                if (newBaseAssetPrice == null)
                    throw new UnprocessableEntityException("The asset symbol to return price in doesn't exist", FailureCode.BaseAssetPriceDoesntExist);

                // Get assets
                var assets = await _assetRepository.GetAssetsReadOnlyAsync(query.AssetType, query.PricingProviderType, true, query.Symbols);
                var assetIds = assets.Select(x => x.Id).ToList();

                // Get prices
                var assetPrices = await _assetPriceRepository.GetLatestAssetPricesReadOnlyAsync(assetIds, query.PricingProviderType, query.AssetType,
                    cancellationToken);

                // Convert to base asset price
                assetPrices.ForEach(x => { x.SetNewBaseAsset(baseAsset, oldBaseAssetPrice, newBaseAssetPrice); });

                return assetPrices;
            }
        }
    }
}
