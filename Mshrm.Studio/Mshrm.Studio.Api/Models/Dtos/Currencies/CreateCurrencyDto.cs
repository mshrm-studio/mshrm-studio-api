using Mshrm.Studio.Api.Clients.Pricing;
using Mshrm.Studio.Api.Models.Dtos.Files;
using Newtonsoft.Json;

namespace Mshrm.Studio.Api.Models.Dtos.Currencies
{
    public class CreateCurrencyDto
    {
        [JsonProperty("name")]
        public string Name {  get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("symbolNative")]
        public string SymbolNative { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("providerType")]
        public PricingProviderType ProviderType { get; set; }

        [JsonProperty("currencyType")]
        public CurrencyType CurrencyType { get; set; }

        [JsonProperty("logo")]
        public TemporaryFileDto? Logo {  get; set; } 
    }
}
