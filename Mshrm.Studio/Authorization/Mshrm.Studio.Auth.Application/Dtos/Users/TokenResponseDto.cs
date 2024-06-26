﻿using Newtonsoft.Json;

namespace Mshrm.Studio.Auth.Application.Dtos.Users
{
    public class TokenResponseDto
    {
        [JsonProperty("tokenValue")]
        public string TokenValue { get; set; }

        [JsonProperty("refreshTokenValue")]
        public string RefreshTokenValue { get; set; }

        [JsonProperty("validTo")]
        public DateTime ValidTo { get; set; }
    }
}
