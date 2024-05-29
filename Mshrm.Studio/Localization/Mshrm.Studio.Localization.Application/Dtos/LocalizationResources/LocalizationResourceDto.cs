using Mshrm.Studio.Localization.Api.Models.Enums;
using Newtonsoft.Json;

namespace Mshrm.Studio.Localization.Api.Models.Dtos.LocalizationResources
{
    public class LocalizationResourceDto
    {
        [JsonProperty("guidId")]
        public Guid GuidId { get; set; }

        [JsonProperty("culture")]
        public string Culture { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("comment")]
        public string? Comment { get; set; }

        [JsonProperty("localizationArea")]
        public LocalizationArea LocalizationArea { get; set; }
    }
}
