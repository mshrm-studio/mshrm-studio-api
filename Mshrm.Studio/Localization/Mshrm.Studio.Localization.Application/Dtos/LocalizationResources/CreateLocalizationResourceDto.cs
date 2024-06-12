using Mshrm.Studio.Localization.Api.Models.Enums;
using Newtonsoft.Json;

namespace Mshrm.Studio.Localization.Api.Models.Dtos.LocalizationResources
{ 
    /// <summary>
    /// Create a new localization resource
    /// </summary>
    public class CreateLocalizationResourceDto
    {
        /// <summary>
        /// The localization culture
        /// </summary>
        [JsonProperty("culture")]
        public required string Culture { get; set; }

        /// <summary>
        /// The key
        /// </summary>
        [JsonProperty("key")]
        public required string Key { get; set; }

        /// <summary>
        /// The value for key
        /// </summary>
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
        [JsonProperty("localizationArea")]
        public LocalizationArea LocalizationArea { get; set; }
    }
}
