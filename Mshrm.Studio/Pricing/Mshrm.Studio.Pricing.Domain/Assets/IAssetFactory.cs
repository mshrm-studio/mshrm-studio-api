using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Domain.Assets
{
    public interface IAssetFactory
    {
        /// <summary>
        /// Create a new asset
        /// </summary>
        /// <param name="providerType">The provider to import asset from</param>
        /// <param name="assetType">The type of asset ie. Fiat</param>
        /// <param name="name">The name of the asset</param>
        /// <param name="symbol">A symbol for the asset</param>
        /// <param name="symbolNative">The display symbol ie. $ for US dollars</param>
        /// <param name="description">A description</param>
        /// <param name="logoGuidId">A logo</param>
        /// <returns>A new asset</returns>
        Asset CreateAsset(PricingProviderType providerType, AssetType assetType, string name, string symbol, string symbolNative, string? description, Guid? logoGuidId);
    }
}
