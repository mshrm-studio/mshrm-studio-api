using Mshrm.Studio.Shared.Attributes;
using Newtonsoft.Json;

namespace Mshrm.Studio.Api.Models.Dtos.Files
{
    public class UploadTemporaryFileDto
    {
        /// <summary>
        /// User profile picture
        /// </summary>
        [JsonProperty("displayImage")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" })]
        [MaxFileSize(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, 1500000)]
        [MinFileSize(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, 10)]
        public IFormFile File { get; set; }
    }
}
