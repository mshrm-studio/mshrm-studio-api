using Newtonsoft.Json;

namespace Mshrm.Studio.Auth.Api.Models.Dtos
{
    public class LoginRequestDto
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
