using Newtonsoft.Json;

namespace Mshrm.Studio.Storage.Api.Models.Dtos.Files
{
    public class TemporaryFileUploadDto
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("expiryDate")]
        public DateTime ExpiryDate { get; set; }
    }
}
