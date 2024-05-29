using Newtonsoft.Json;

namespace Mshrm.Studio.Pricing.Api.Models.HttpService.PolygonIO
{
    /// <summary>
    /// For PolygonIO get price
    /// </summary>
    public class PolygonIOPriceResponse
    {
        /// <summary>
        /// The symbol
        /// </summary>
        [JsonProperty("symbol")]
        public required string Symbol { get; set; }

        /// <summary>
        /// The opening value of trades
        /// </summary>
        [JsonProperty("open")]
        public decimal Open { get; set; }

        /// <summary>
        /// The close value of trades
        /// </summary>
        [JsonProperty("close")]
        public decimal? Close { get; set; }

        /// <summary>
        /// The volume of trades
        /// </summary>
        [JsonProperty("volume")]
        public decimal Volume { get; set; }
    }
}
