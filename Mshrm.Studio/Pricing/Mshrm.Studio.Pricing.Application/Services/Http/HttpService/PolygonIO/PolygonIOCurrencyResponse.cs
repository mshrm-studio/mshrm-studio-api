using Newtonsoft.Json;

namespace Mshrm.Studio.Pricing.Api.Models.HttpService.PolygonIO
{
    public class PolygonIOCurrencyResponse
    {
        [JsonProperty("results")]
        public List<PolygonIOCurrency> Currencies { get; set; }
    }
}
