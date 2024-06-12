using Mshrm.Studio.Api.Clients.Localization;
using Mshrm.Studio.Shared.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Mshrm.Studio.Api.Models.Dtos.Localization
{
    public class CreateLocalizationResourceRequestDto
    {
        /// <summary>
        /// The localization culture
        /// </summary>
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("culture")]
        public required string Culture { get; set; }

        /// <summary>
        /// The key
        /// </summary>
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("key")]
        public required string Key { get; set; }

        /// <summary>
        /// The value for key
        /// </summary>
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("value")]
        public required string Value { get; set; }

        /// <summary>
        /// Any comments
        /// </summary>
        [JsonProperty("comment")]
        public string? Comment { get; set; }

        /// <summary>
        /// A category for localization
        /// </summary>
        [Required(ErrorMessage = "MissingBindRequiredValueAccessor")]
        [JsonProperty("localizationArea")]
        public required LocalizationArea LocalizationArea { get; set; }
    }
}
