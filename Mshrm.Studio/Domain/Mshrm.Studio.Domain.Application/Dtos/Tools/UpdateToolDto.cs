using Mshrm.Studio.Domain.Api.Models.Enums;
using Newtonsoft.Json;

namespace Mshrm.Studio.Domain.Api.Models.Dtos.Tools
{
    /// <summary>
    /// Update a tool
    /// </summary>
    public class UpdateToolDto
    {
        /// <summary>
        /// The name of the tool
        /// </summary>
        [JsonProperty("name")]
        public required string Name { get; set; }

        /// <summary>
        /// A description
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
        /// A link to info/source code
        /// </summary>
        [JsonProperty("link")]
        public required string Link { get; set; }

        /// <summary>
        /// The logo GUID id
        /// </summary>
        [JsonProperty("logoGuidId")]
        public Guid? LogoGuidId { get; set; }
    }
}
