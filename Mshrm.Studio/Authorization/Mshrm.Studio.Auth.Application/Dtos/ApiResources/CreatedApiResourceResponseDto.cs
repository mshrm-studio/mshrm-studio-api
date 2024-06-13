using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Application.Dtos.ApiResources
{
    public class CreatedApiResourceResponseDto
    {
        [JsonProperty("apiResource")]
        public ApiResourceResponseDto ApiResource { get; set; }

        [JsonProperty("plainTextSecret")]
        public string PlainTextSecret { get; set; }
    }
}
