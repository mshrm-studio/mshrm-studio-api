using Newtonsoft.Json;

namespace Mshrm.Studio.Pricing.Api.Models.HttpService.Mobula
{
    public class MobulaCurrencyResponse
    {
        [JsonProperty("data")]
        public List<MobulaCurrency> Currencies { get; set; }
    }
}
