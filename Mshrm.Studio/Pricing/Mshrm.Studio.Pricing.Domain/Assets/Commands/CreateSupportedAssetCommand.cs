using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Models.CQRS.Assets.Commands
{
    public class CreateSupportedAssetCommand : IRequest<Asset>
    {
        public string Name { get; private set; }
        public string Symbol { get; set; }
        public string SymbolNative { get; set; }
        public string? Description { get; set; }
        public PricingProviderType ProviderType { get; set; }
        public AssetType AssetType { get; set; }
        public Guid? LogoGuidId { get; set; }
    }
}
