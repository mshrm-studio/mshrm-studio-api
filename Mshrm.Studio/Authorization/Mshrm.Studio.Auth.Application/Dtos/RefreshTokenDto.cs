using Newtonsoft.Json;

namespace Mshrm.Studio.Auth.Api.Models.Dtos
{
    public class RefreshTokenDto
    {
        /// <summary>
        /// The expired token. 
        /// Example: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtYXR0c2hhcn
        /// </summary>
        [JsonProperty("tokenValue")]
        public string TokenValue { get; set; }

        /// <summary>
        /// The refresh token. 
        /// Example: HyRz/mK5xreL1NPXIEV89PSiFWhPO4lBEAMh1z0nPdQ=
        /// </summary>
        [JsonProperty("refreshTokenValue")]
        public string RefreshTokenValue { get; set; }
    }
}
