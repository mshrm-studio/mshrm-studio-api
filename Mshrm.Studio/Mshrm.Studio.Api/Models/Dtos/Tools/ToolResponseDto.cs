using Mshrm.Studio.Api.Clients.Domain;
using Newtonsoft.Json;

namespace Mshrm.Studio.Api.Models.Dtos.Tools
{
    /// <summary>
    /// Response for a tool
    /// </summary>
    public class ToolResponseDto
    {
        /// <summary>
        /// The identifier for the tool
        /// </summary>
        [JsonProperty("guidId")]
        public Guid GuidId { get; set; }

        /// <summary>
        /// The name of the tool
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The tools description
        /// </summary>
        [JsonProperty("description")]
        public string? Description { get; set; }

        /// <summary>
        /// The type of tool
        /// </summary>
        [JsonProperty("toolType")]
        public ToolType ToolType { get; set; }

        /// <summary>
        /// The display rank
        /// </summary>
        [JsonProperty("rank")]
        public int Rank { get; set; }

        /// <summary>
        /// A link to the tool
        /// </summary>
        [JsonProperty("link")]
        public string Link { get; set; }

        /// <summary>
        /// Logo GUID
        /// </summary>
        [JsonProperty("logoGuidId")]
        public Guid? logoGuidId { get; set; }

        /// <summary>
        /// Logo URL
        /// </summary>
        [JsonProperty("logoUrl")]
        public string? LogoUrl { get; set; }
    }
}
