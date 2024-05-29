using Newtonsoft.Json;

namespace Mshrm.Studio.Auth.Api.Models.Dtos
{
    public class PasswordResetTokenRequestDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
