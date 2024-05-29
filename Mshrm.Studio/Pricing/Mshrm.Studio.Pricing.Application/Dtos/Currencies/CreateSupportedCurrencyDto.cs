using Mshrm.Studio.Pricing.Api.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Pricing.Api.Models.Dtos.Currency
{
    public class CreateSupportedCurrencyDto
    {
        [JsonProperty("name")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        public string Name { get; private set; }

        [JsonProperty("symbol")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        public string Symbol { get; set; }

        [JsonProperty("symbolNative")]
        [StringLength(256, ErrorMessage = "{0} must have less than {1} characters")]
        public string SymbolNative { get; set; }

        [JsonProperty("description")]
        [StringLength(1000, ErrorMessage = "{0} must have less than {1} characters")]
        public string? Description { get; set; }

        [JsonProperty("providerType")]
        public PricingProviderType ProviderType { get; set; }

        [JsonProperty("currencyType")]
        public CurrencyType CurrencyType { get; set; }

        [JsonProperty("logoGuidId")]
        public Guid? LogoGuidId { get; set; }
    }
}
