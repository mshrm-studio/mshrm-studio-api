using Newtonsoft.Json;

namespace Mshrm.Studio.Auth.Application.Dtos.Users
{
    public class PasswordResetTokenRequestDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
