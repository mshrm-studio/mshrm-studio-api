using Mshrm.Studio.Pricing.Api.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Pricing.Api.Models.Dtos.Currencies
{
    /// <summary>
    /// Update model for currency
    /// </summary>
    public class UpdateSupportedCurrencyDto
    {
        /// <summary>
        /// THe currency name
        /// </summary>
        [JsonProperty("name")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        public required string Name { get; set; }

        /// <summary>
        /// The native symbol ie. USD -> $
        /// </summary>
        [JsonProperty("symbolNative")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        public required string SymbolNative { get; set; }

        /// <summary>
        /// The description for the currency
        /// </summary>
        [JsonProperty("description")]
        [StringLength(1000, ErrorMessage = "{0} must have less than {1} characters")]
        public string? Description { get; set; }

        /// <summary>
        /// THe provider for currency import
        /// </summary>
        [JsonProperty("providerType")]
        public PricingProviderType ProviderType { get; set; }

        /// <summary>
        /// The type of currency
        /// </summary>
        [JsonProperty("currencyType")]
        public CurrencyType CurrencyType { get; set; }

        /// <summary>
        /// The supported currencies logo GUID
        /// </summary>
        [JsonProperty("logoGuidId")]
        public Guid? LogoGuidId { get; set; }
    }
}
