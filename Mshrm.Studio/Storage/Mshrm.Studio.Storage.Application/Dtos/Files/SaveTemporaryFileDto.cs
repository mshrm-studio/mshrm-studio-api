using Newtonsoft.Json;

namespace Mshrm.Studio.Storage.Api.Models.Dtos.Files
{
    public class SaveTemporaryFileDto
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("isPrivate")]
        public bool IsPrivate { get; set; } = false;
    }
}
