using Mshrm.Studio.Pricing.Api.Models.Dtos.Asset;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Application.Dtos.AssetPriceHistories
{
    public class AssetPriceHistoryDto
    {
        [JsonProperty("oldPrice")]
        public decimal OldPrice { get; set; }

        [JsonProperty("oldMarketCap")]
        public decimal? OldMarketCap { get; set; }

        [JsonProperty("oldVolume")]
        public decimal? OldVolume { get; set; }

        [JsonProperty("newPrice")]
        public decimal NewPrice { get; set; }

        [JsonProperty("newMarketCap")]
        public decimal? NewMarketCap { get; set; }

        [JsonProperty("newVolume")]
        public decimal? NewVolume { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }
    }
}
