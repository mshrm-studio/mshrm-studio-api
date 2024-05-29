using Mshrm.Studio.Domain.Api.Models.Enums;
using Newtonsoft.Json;

namespace Mshrm.Studio.Domain.Api.Models.Dtos.Tools
{
    public class ToolDto
    {
        [JsonProperty("guidId")]
        public Guid GuidId { get; set; }

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

        [JsonProperty("logoGuidId")]
        public Guid? LogoGuidId { get; set; }
    }
}
