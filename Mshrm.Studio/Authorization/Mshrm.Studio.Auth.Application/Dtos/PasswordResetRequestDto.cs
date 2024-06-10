using Newtonsoft.Json;

namespace Mshrm.Studio.Auth.Api.Models.Dtos
{
    public class PasswordResetRequestDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("newPassword")]
        public string NewPassword { get; set; }
    }
}
