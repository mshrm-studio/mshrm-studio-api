using Newtonsoft.Json;

namespace Mshrm.Studio.Storage.Api.Models.Dtos.Resources
{
    public class UploadResourceDto
    {
        [JsonProperty("temporaryFileKey")]
        public string TemporaryFileKey { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }
    }
}
