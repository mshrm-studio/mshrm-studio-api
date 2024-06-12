using MediatR;
using Microsoft.Extensions.Logging;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.CQRS.AssetPrices.Commands;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Domain.AssetPrices;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using OpenTracing;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.AssetPrices
{
    /// <summary>
    /// Create an asset price
    /// </summary>
    public class CreateOrReplaceAssetPricesCommandHandler : IRequestHandler<CreateOrReplaceAssetPricesCommand, List<AssetPrice>>
    {
        private readonly IAssetPriceRepository _assetPriceRepository;
        private readonly IAssetPriceFactory _assetPriceFactory;
        private readonly IAssetRepository _assetRepository;
        private readonly ILogger<CreateOrReplaceAssetPricesCommandHandler> _logger;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrReplaceAssetPricesCommandHandler"/> class.
        /// </summary>
        /// <param name="assetPriceRepository"></param>
        /// <param name="assetRepository"></param>
        /// <param name="logger"></param>
        /// <param name="tracer"></param>
        public CreateOrReplaceAssetPricesCommandHandler(IAssetPriceRepository assetPriceRepository, IAssetRepository assetRepository, IAssetPriceFactory assetPriceFactory,
            ILogger<CreateOrReplaceAssetPricesCommandHandler> logger, ITracer tracer)
        {
            _assetPriceFactory = assetPriceFactory;
            _assetPriceRepository = assetPriceRepository;
            _assetRepository = assetRepository;

            _tracer = tracer;
            _logger = logger;
        }

        /// <summary>
        /// Create or replace existing asset prices in database
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The prices added/updated</returns>
        public async Task<List<AssetPrice>> Handle(CreateOrReplaceAssetPricesCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("CreateOrReplaceAssetPricesCommandHandler").StartActive(true))
            {
                // Get all the assets for the symbols provided
                var allSymbols = command.Prices.Select(x => x.Asset).ToList();
                var allBaseSymbols = command.Prices.Select(x => x.BaseAsset).ToList();
                var allAssets = await _assetRepository.GetAssetsReadOnlyAsync(null, null, true, allSymbols);
                var allAssetIds = allAssets.Select(x => x.Id).ToList();
                var baseAssets = await _assetRepository.GetAssetsReadOnlyAsync(null, null, true, allBaseSymbols);

                var existingAssetPrices = await _assetPriceRepository.GetLatestAssetPricesReadOnlyAsync(allAssetIds, null, null, cancellationToken);

                var allAssetPrices = new List<AssetPrice>();
                var allAssetPricesToAdd = new List<AssetPrice>();
                var allAssetPricesToUpdate = new List<AssetPrice>();

                foreach (var assetPrice in command.Prices)
                {
                    // Check the price doesn't already exist, if so then its replaced
                    var existingPrice = existingAssetPrices.FirstOrDefault(x => x.Asset.Symbol == assetPrice.Asset);
                    if (existingPrice != null)
                    {
                        existingPrice.SetPrice(assetPrice.Price);
                        allAssetPricesToUpdate.Add(existingPrice);
                    }
                    else
                    {
                        // Get the base asset
                        var baseAsset = baseAssets.FirstOrDefault(x => x.Symbol == assetPrice.BaseAsset);
                        if (baseAsset == null)
                        {
                            _logger.LogCritical($"Base asset: {assetPrice.BaseAsset} doesn't exist");

                            throw new NotFoundException("Base asset doesn't exist", FailureCode.AssetDoesntExist);
                        }

                        // Get the asset
                        var asset = allAssets.FirstOrDefault(x => x.Symbol == assetPrice.Asset);
                        if (asset == null)
                        {
                            _logger.LogCritical($"Asset: {assetPrice.Asset} doesn't exist");

                            throw new NotFoundException("Asset doesn't exist", FailureCode.AssetDoesntExist);
                        }

                        allAssetPricesToAdd.Add(_assetPriceFactory.CreateAssetPrice(baseAsset.Id, asset.Id, assetPrice.Price, assetPrice.MarketCap, assetPrice.Volume, command.PricingProviderType));
                    }
                }

                // Add any updated ones
                var updated = await _assetPriceRepository.UpdateAssetPricesAsync(allAssetPricesToUpdate, command.PricingProviderType, cancellationToken);
                if (updated.Any())
                {
                    allAssetPrices.AddRange(updated);
                }

                // Add any new ones
                var added = await _assetPriceRepository.CreateAssetPricesAsync(allAssetPricesToAdd, cancellationToken);
                if (added.Any())
                {
                    allAssetPrices.AddRange(added);
                }

                return allAssetPricesToUpdate;
            }
        }
    }
}
