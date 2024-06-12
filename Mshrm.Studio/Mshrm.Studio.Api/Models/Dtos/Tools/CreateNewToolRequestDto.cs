using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Models.Dtos.Files;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Api.Models.Dtos.Tools
{
    public class CreateNewToolRequestDto
    {
        /// <summary>
        /// The tool for the name
        /// </summary>
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("name")]
        public required string Name { get; set; }

        /// <summary>
        /// A tool description
        /// </summary>
        [JsonProperty("description")]
        public string? Description { get; set; }

        /// <summary>
        /// The type of tool
        /// </summary>
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("toolType")]
        public required ToolType ToolType { get; set; }

        /// <summary>
        /// The rank in which its shown
        /// </summary>
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("rank")]
        public required int Rank { get; set; }

        /// <summary>
        /// A link to the tool (homepage)
        /// </summary>
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("link")]
        public required string Link { get; set; }

        /// <summary>
        /// The tool logo
        /// </summary>
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("logo")]
        public required TemporaryFileRequestDto Logo { get; set; }
    }
}
