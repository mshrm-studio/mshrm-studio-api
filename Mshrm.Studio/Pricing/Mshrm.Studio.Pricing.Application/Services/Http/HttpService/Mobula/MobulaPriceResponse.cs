using Newtonsoft.Json;

namespace Mshrm.Studio.Pricing.Api.Models.HttpService.Mobula
{
    public class MobulaPriceResponse
    {
        [JsonProperty("data")]
        public List<MobulaPricePair> PricePairs { get; set; } = new List<MobulaPricePair>();
    }
}
