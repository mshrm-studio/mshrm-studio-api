using Newtonsoft.Json;

namespace Mshrm.Studio.Auth.Application.Dtos.Users
{
    public class LoginRequestDto
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
