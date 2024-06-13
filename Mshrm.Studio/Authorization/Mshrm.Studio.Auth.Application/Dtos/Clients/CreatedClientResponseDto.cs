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
    public class CreatedClientResponseDto
    {
        [JsonProperty("client")]
        public ClientResponseDto? Client { get; set; }

        [JsonProperty("plainTextSecret")]
        public string PlainTextSecret { get; set; }
    }
}
