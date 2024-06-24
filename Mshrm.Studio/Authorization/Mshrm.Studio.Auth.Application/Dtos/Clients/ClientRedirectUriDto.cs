using Duende.IdentityServer.EntityFramework.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Application.Dtos.Clients
{
    public class ClientRedirectUriDto
    {
        [JsonProperty("redirectUri")]
        public string RedirectUri { get; set; }
    }
}
