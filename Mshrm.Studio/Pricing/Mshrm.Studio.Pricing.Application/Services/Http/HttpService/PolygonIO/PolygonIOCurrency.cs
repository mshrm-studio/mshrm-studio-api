using Newtonsoft.Json;

namespace Mshrm.Studio.Pricing.Api.Models.HttpService.PolygonIO
{
    /// <summary>
    /// PolygonIO currency
    /// </summary>
    public class PolygonIOCurrency
    {
        /// <summary>
        /// The name
        /// </summary>
        [JsonProperty("name")]
        public required string Name { get; set; }

        /// <summary>
        /// The symbol
        /// </summary>
        [JsonProperty("ticker")]
        public required string Ticker { get; set; }
    }
}
