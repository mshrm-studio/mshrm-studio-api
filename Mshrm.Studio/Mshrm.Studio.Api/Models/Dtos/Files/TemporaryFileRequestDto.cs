using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Api.Models.Dtos.Files
{
    /// <summary>
    /// A temporary file
    /// </summary>
    public class TemporaryFileRequestDto
    {
        /// <summary>
        /// The temp key
        /// </summary>
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("temporaryKey")]
        public required string TemporaryKey { get; set; }

        /// <summary>
        /// The temp file name
        /// </summary>
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("fileName")]
        public required string FileName { get; set; }
    }
}
