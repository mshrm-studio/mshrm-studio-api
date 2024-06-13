using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Application.Dtos.ApiResources
{
    public class ApiResourceScopeResponseDto
    {
        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("apiResourceId")]
        public int ApiResourceId { get; set; }
    }
}
