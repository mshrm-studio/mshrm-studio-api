using Mshrm.Studio.Auth.Application.Dtos.Clients;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Application.Dtos.ApiResources
{
    public class ApiResourceResponseDto
    {
        [JsonProperty("scopes")]
        public List<ApiResourceScopeResponseDto> Scopes { get; set; }

        [JsonProperty("secrets")]
        public List<ApiResourceSecretResponseDto> Secrets { get; set; }

        [JsonProperty("userClaims")]
        public List<ApiResourceUserClaimResponseDto> UserClaims { get; set; }
    }
}
