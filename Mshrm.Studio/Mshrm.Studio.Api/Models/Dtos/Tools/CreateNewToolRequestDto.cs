using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Models.Dtos.Files;
using Newtonsoft.Json;

namespace Mshrm.Studio.Api.Models.Dtos.Tools
{
    public class CreateNewToolRequestDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("toolType")]
        public ToolType ToolType { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("logo")]
        public TemporaryFileRequestDto Logo { get; set; }
    }
}
