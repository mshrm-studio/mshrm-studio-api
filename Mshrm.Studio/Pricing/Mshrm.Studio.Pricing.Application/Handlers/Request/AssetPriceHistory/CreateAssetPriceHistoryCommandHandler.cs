using MediatR;
using Microsoft.Extensions.Logging;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.CQRS.AssetPriceHistories.Commands;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Application.Handlers.Request.AssetPrices;
using Mshrm.Studio.Pricing.Domain.AssetPriceHistories;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using OpenTracing;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.AssetPriceHistories
{
    /// <summary>
    /// Create an asset price
    /// </summary>
    public class CreateAssetPriceHistoryCommandHandler : IRequestHandler<CreateAssetPriceHistoryCommand, AssetPriceHistory>
    {
        private readonly IAssetPriceHistoryRepository _assetPriceHistoryRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly ILogger<CreateOrReplaceAssetPricesCommandHandler> _logger;
        private readonly ITracer _tracer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrReplaceAssetPricesCommandHandler"/> class.
        /// </summary>
        /// <param name="assetPriceHistoryRepository"></param>
        /// <param name="assetRepository"></param>
        /// <param name="logger"></param>
        /// <param name="tracer"></param>
        public CreateAssetPriceHistoryCommandHandler(IAssetPriceHistoryRepository assetPriceHistoryRepository, IAssetRepository assetRepository, 
            ILogger<CreateOrReplaceAssetPricesCommandHandler> logger, ITracer tracer)
        {
            _assetPriceHistoryRepository = assetPriceHistoryRepository;
            _assetRepository = assetRepository;

            _tracer = tracer;
            _logger = logger;
        }

        /// <summary>
        /// Add a history item
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">The stopping token</param>
        /// <returns>The prices added/updated</returns>
        public async Task<AssetPriceHistory> Handle(CreateAssetPriceHistoryCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("AddHistoryAsync_CreatePriceHistoryService").StartActive(true))
            {
                return await _assetPriceHistoryRepository.AddAssetPriceHistoryAsync(command.Id, command.OldPrice, command.NewPrice, command.OldMarketCap, command.NewMarketCap,
                    command.OldVolume, command.NewVolume, command.PricingProviderType, cancellationToken);
            }
        }
    }
}
