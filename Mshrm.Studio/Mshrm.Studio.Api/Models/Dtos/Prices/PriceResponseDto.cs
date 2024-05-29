using Mshrm.Studio.Api.Models.Dtos.Currencies;
using Newtonsoft.Json;

namespace Mshrm.Studio.Api.Models.Dtos.Prices
{
    public class PriceResponseDto
    {
        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("marketCap")]
        public decimal? MarketCap { get; set; }

        [JsonProperty("volume")]
        public decimal? Volume { get; set; }

        [JsonProperty("baseCurrency")]
        public CurrencyResponseDto BaseCurrency { get; set; }

        [JsonProperty("currency")]
        public CurrencyResponseDto Currency { get; set; }
    }
}
