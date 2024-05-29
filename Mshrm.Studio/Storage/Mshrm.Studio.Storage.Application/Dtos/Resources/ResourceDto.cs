using Mshrm.Studio.Storage.Api.Models.Enums;
using Newtonsoft.Json;

namespace Mshrm.Studio.Storage.Api.Models.Dtos.Resources
{
    public class ResourceDto
    {
        [JsonProperty("guidId")]
        public Guid GuidId { get; set; }

        [JsonProperty("assetType")]
        public AssetType AssetType { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("extension")]
        public string Extension { get; set; }
    }
}
