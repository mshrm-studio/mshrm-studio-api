using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Application.Dtos.Clients
{
    public class ApiResourceSecretResponseDto
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("expiration")]
        public DateTime? Expiration { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }
    }
}
