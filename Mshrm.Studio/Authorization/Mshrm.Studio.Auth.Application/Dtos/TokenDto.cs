using Newtonsoft.Json;

namespace Mshrm.Studio.Auth.Api.Models.Dtos
{
    public class TokenDto
    {
        [JsonProperty("tokenValue")]
        public string TokenValue { get; set; }

        [JsonProperty("refreshTokenValue")]
        public string RefreshTokenValue { get; set; }

        [JsonProperty("validTo")]
        public DateTime ValidTo { get; set; }
    }
}
