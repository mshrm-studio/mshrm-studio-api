using IdentityServer4.Models;
using Mshrm.Studio.Auth.Domain.Clients.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Application.Dtos.Clients
{
    public class ClientResponseDto
    {
        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("clientName")]
        public string ClientName { get; set; }

        [JsonProperty("protocolType")]
        public string ProtocolType { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("clientSecrets")]
        public List<ClientSecretResponseDto> ClientSecrets { get; set; }

        [JsonProperty("allowedGrantTypes")]
        public List<AllowedGrantType> AllowedGrantTypes { get; set; } = new List<AllowedGrantType>();

        [JsonProperty("redirectUris")]
        public List<ClientRedirectUriDto> ClientRedirectUris { get; set; } = new List<ClientRedirectUriDto>();
    }
}
