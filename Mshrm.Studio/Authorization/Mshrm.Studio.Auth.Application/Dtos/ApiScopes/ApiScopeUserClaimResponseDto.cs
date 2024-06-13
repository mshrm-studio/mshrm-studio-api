using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Application.Dtos.ApiResources
{
    public class ApiScopeUserClaimResponseDto
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("scopeId")]
        public int ScopeId { get; set; }
    }
}
