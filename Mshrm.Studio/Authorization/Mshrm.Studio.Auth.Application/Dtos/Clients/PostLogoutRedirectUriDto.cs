using Duende.IdentityServer.EntityFramework.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Application.Dtos.Clients
{
    public class PostLogoutRedirectUriDto
    {
        [JsonProperty("postLogoutRedirectUri")]
        public string PostLogoutRedirectUri { get; set; }
    }
}
