using Mshrm.Studio.Shared.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Api.Models.Dtos.Files
{
    public class UploadTemporaryFileRequestDto
    {
        /// <summary>
        /// User profile picture
        /// </summary>
        [JsonProperty("displayImage")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" })]
        [MaxFileSize(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, 1500000)]
        [MinFileSize(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, 10)]
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        public required IFormFile File { get; set; }
    }
}
