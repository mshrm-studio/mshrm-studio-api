using Mshrm.Studio.Auth.Domain.Clients.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Application.Dtos.Clients
{
    public class CreateClientRequestDto
    {
        [JsonProperty("idName")]
        public string IdName { get; set; }

        [JsonProperty("clientName")]
        public string ClientName { get; set; }

        [JsonProperty("grantTypes")]
        public List<AllowedGrantType> GrantTypes { get; set; } = new List<AllowedGrantType>();

        [JsonProperty("scopes")]
        public List<string> Scopes { get; set; } = new List<string>();
    }
}
