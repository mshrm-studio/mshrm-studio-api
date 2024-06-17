using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Application.Dtos.ApiResources
{
    public class CreateApiScopeRequestDto
    {
        [JsonProperty("name")]
        public required string Name { get; set; }

        [JsonProperty("scopes")]
        public List<string> Scopes { get; set; }
    }
}
