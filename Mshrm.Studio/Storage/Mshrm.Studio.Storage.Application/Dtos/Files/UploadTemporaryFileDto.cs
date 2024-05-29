using Microsoft.AspNetCore.Http;
using Mshrm.Studio.Shared.Attributes;
using Newtonsoft.Json;

namespace Mshrm.Studio.Storage.Api.Models.Dtos.Files
{
    public class UploadTemporaryFileDto
    {
        /// <summary>
        /// User profile picture
        /// </summary>
        [JsonProperty("displayImage")]
        [MaxFileSize(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, 1500000)]
        [MinFileSize(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, 10)]
        public IFormFile File { get; set; }
    }
}
