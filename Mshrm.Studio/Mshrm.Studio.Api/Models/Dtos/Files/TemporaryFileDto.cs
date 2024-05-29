using Newtonsoft.Json;

namespace Mshrm.Studio.Api.Models.Dtos.Files
{
    /// <summary>
    /// A temporary file
    /// </summary>
    public class TemporaryFileDto
    {
        /// <summary>
        /// The temp key
        /// </summary>
        [JsonProperty("temporaryKey")]
        public required string TemporaryKey { get; set; }

        /// <summary>
        /// The temp file name
        /// </summary>
        [JsonProperty("fileName")]
        public required string FileName { get; set; }
    }
}
