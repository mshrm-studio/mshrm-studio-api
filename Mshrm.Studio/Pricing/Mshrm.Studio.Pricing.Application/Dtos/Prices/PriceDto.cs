using Mshrm.Studio.Pricing.Api.Models.Dtos.Currency;
using Newtonsoft.Json;

namespace Mshrm.Studio.Pricing.Api.Models.Dtos.Prices
{
    public class PriceDto
    {
        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("marketCap")]
        public decimal? MarketCap { get; set; }

        [JsonProperty("volume")]
        public decimal? Volume { get; set; }

        [JsonProperty("baseCurrency")]
        public CurrencyDto BaseCurrency { get; set; }

        [JsonProperty("currency")]
        public CurrencyDto Currency { get; set; }
    }
}
