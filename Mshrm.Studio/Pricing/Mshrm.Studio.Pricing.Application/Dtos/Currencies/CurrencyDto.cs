using Mshrm.Studio.Pricing.Api.Models.Enums;
using Newtonsoft.Json;

namespace Mshrm.Studio.Pricing.Api.Models.Dtos.Currency
{
    public class CurrencyDto
    {
        [JsonProperty("guidId")]
        public Guid GuidId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("symbolNative")]
        public string SymbolNative { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("providerType")]
        public PricingProviderType ProviderType { get; set; }

        [JsonProperty("currencyType")]
        public CurrencyType CurrencyType { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("logoGuidId")]
        public Guid? LogoGuidId { get; private set; }
    }
}
