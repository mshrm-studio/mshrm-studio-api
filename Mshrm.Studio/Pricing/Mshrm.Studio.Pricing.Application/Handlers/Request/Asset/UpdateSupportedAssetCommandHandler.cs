using MediatR;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Assets.Commands;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Providers;
using Mshrm.Studio.Pricing.Application.Services.Providers;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Models.Pagination;
using OpenTracing;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.Assets
{
    public class UpdateSupportedAssetCommandHandler : IRequestHandler<UpdateSupportedAssetCommand, Asset>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly AssetPriceServiceResolver _assetPriceServiceResolver;
        private readonly ITracer _tracer;

        public UpdateSupportedAssetCommandHandler(IAssetRepository assetRepository, AssetPriceServiceResolver assetPriceServiceResolver, ITracer tracer)
        {
            _assetRepository = assetRepository;

            _tracer = tracer;
            _assetPriceServiceResolver = assetPriceServiceResolver;
        }

        /// <summary>
        /// Update an asset (all except symbol)
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The updated asset</returns>
        public async Task<Asset> Handle(UpdateSupportedAssetCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("CreateAssetAsync_CreateAssetService").StartActive(true))
            {
                // Check symbol already exists for asset type
                var existingAsset = await _assetRepository.GetAssetAsync(command.AssetId, true, cancellationToken);
                if (existingAsset == null)
                    throw new NotFoundException("The asset doesn't exist", FailureCode.AssetDoesntExist);

                // Check provider can support asset import
                var provider = _assetPriceServiceResolver(command.ProviderType);
                var isSupportedByProvider = await provider.IsAssetSupportedAsync(existingAsset.Symbol);
                if (!isSupportedByProvider)
                    throw new UnprocessableEntityException("Asset is not supported by the provider", FailureCode.AssetNotSupported);

                // Update
                return await _assetRepository.UpdateAssetAsync(command.AssetId, command.Name, command.Description, command.ProviderType,
                    command.AssetType, command.SymbolNative, command.LogoGuidId, cancellationToken);
            }
        }
    }
}
