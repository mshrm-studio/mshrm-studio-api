using Newtonsoft.Json;

namespace Mshrm.Studio.Storage.Api.Models.Dtos.Files
{
    public class SaveTemporaryFilesDto
    {
        [JsonProperty("temporaryFileKeys")]
        public List<SaveTemporaryFileDto> TemporaryFileKeys { get; set; }
    }
}
