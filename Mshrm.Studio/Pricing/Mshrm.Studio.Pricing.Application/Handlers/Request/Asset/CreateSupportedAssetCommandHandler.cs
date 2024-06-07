using MediatR;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Assets.Commands;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairs.Commands;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Providers;
using Mshrm.Studio.Pricing.Application.Services.Providers;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using OpenTracing;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.Assets
{
    public class CreateSupportedAssetCommandHandler : IRequestHandler<CreateSupportedAssetCommand, Asset>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly ITracer _tracer;
        private readonly AssetPriceServiceResolver _assetPriceServiceResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSupportedAssetCommandHandler"/> class.
        /// </summary>
        /// <param name="assetRepository"></param>
        /// <param name="assetPriceServiceResolver"></param>
        /// <param name="tracer"></param>
        public CreateSupportedAssetCommandHandler(AssetPriceServiceResolver assetPriceServiceResolver, IAssetRepository assetRepository, ITracer tracer)
        {
            _assetRepository = assetRepository;
            _assetPriceServiceResolver = assetPriceServiceResolver;

            _tracer = tracer;
        }

        /// <summary>
        /// Create a new asset
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The new asset</returns>
        public async Task<Asset> Handle(CreateSupportedAssetCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("CreateAssetAsync_CreateAssetService").StartActive(true))
            {
                // Check symbol already exists for asset type
                var existingAsset = await _assetRepository.GetAssetAsync(command.Symbol, command.AssetType, true, cancellationToken);
                if (existingAsset != null)
                    throw new UnprocessableEntityException("The asset already exists", FailureCode.AssetAlreadyExists);

                // Check provider can support asset import
                var provider = _assetPriceServiceResolver(command.ProviderType);
                var isSupportedByProvider = await provider.IsAssetSupportedAsync(command.Symbol);
                if (!isSupportedByProvider)
                    throw new UnprocessableEntityException("Asset is not supported by the provider", FailureCode.AssetNotSupported);

                // Create
                return await _assetRepository.CreateAssetAsync(command.Name, command.Description, command.ProviderType, command.AssetType, command.Symbol,
                    command.SymbolNative, command.LogoGuidId, command.DecimalPlaces, cancellationToken);
            }
        }
    }
}
