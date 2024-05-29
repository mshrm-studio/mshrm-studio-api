using Newtonsoft.Json;

namespace Mshrm.Studio.Api.Models.Dtos.Files
{
    public class TemporaryFileUploadResponseDto
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("expiryDate")]
        public DateTime ExpiryDate { get; set; }
    }
}
