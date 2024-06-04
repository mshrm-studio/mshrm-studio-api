using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Shared.Models.Entities;
using Mshrm.Studio.Shared.Models.Entities.Bases;
using Mshrm.Studio.Shared.Models.Entities.Interfaces;

namespace Mshrm.Studio.Pricing.Api.Models.Entites
{
    /// <summary>
    /// An asset
    /// </summary>
    [Index("Symbol", "ProviderType", "Active")]
    public class Asset : AuditableEntity, IAggregateRoot
    {
        /// <summary>
        /// The ID
        /// </summary>
        public int Id { get; private set; }
        
        /// <summary>
        /// Display GUID id
        /// </summary>
        public Guid GuidId { get; private set; }

        /// <summary>
        /// The name 
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The symbol
        /// </summary>
        public string Symbol { get; private set; }

        /// <summary>
        /// The native symbol
        /// </summary>
        public string SymbolNative { get; private set; }

        /// <summary>
        /// A description
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// If the asset is active
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// Logo GUID
        /// </summary>
        public Guid? LogoGuidId { get; private set; }

        /// <summary>
        /// A provider type (for price import)
        /// </summary>
        public PricingProviderType ProviderType { get; private set; }

        /// <summary>
        /// The type
        /// </summary>
        public AssetType AssetType { get; private set; }

        /// <summary>
        /// When the asset was created
        /// </summary>
        public new DateTime CreatedDate { get; private set; }

        /// <summary>
        /// All base prices the asset is used for
        /// </summary>
        public List<ExchangePricingPair> BasePricingPairs { get; private set; } = new List<ExchangePricingPair>();

        /// <summary>
        /// All prices the asset is used for
        /// </summary>
        public List<ExchangePricingPair> PricingPairs { get; private set; } = new List<ExchangePricingPair>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Asset"/> class.
        /// </summary>
        /// <param name="providerType"></param>
        /// <param name="assetType"></param>
        /// <param name="name"></param>
        /// <param name="symbol"></param>
        /// <param name="symbolNative"></param>
        /// <param name="description"></param>
        /// <param name="logoGuidId"></param>
        public Asset(PricingProviderType providerType, AssetType assetType, string name, string symbol, string symbolNative, string? description, Guid? logoGuidId)
        {
            Active = true;
            ProviderType = providerType;
            AssetType = assetType;    
            Name = name;
            Symbol = symbol;
            SymbolNative = symbolNative;
            Description = description;
            CreatedDate = DateTime.UtcNow;
            LogoGuidId = logoGuidId;
        }

        /// <summary>
        /// Set a name 
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            Name = name;
        }

        public void SetDescription(string? description)
        {
            Description = description;
        }

        public void SetProviderType(PricingProviderType providerType)
        {
            ProviderType = providerType;
        }

        public void SetAssetType(AssetType assetType)
        {
            AssetType = assetType;
        }

        public void SetSymbolNative(string symbolNative)
        {
            SymbolNative = symbolNative;
        }

        public void SetLogo(Guid? logoGuidId)
        {
            LogoGuidId = logoGuidId;
        }
    }
}
