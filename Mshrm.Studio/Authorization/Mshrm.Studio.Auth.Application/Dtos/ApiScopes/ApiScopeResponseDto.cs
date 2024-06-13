using Mshrm.Studio.Auth.Application.Dtos.Clients;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Application.Dtos.ApiResources
{
    public class ApiScopeResponseDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("displayName")]
        public string? DisplayName { get; set; }

        [JsonProperty("userClaims")]
        public List<ApiScopeUserClaimResponseDto> UserClaims { get; set; } = new List<ApiScopeUserClaimResponseDto>();
    }
}
