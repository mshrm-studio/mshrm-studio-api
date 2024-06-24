using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Enums;
using System.Security.Claims;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using MediatR;
using Mshrm.Studio.Auth.Domain.Clients.Enums;
using Duende.IdentityServer.EntityFramework.Entities;
using Newtonsoft.Json;

namespace Mshrm.Studio.Auth.Domain.Clients.Commands
{
    public class CreateClientCommand : IRequest<ClientWithSecret>
    {
        public string IdName { get; set; }
        public string ClientName { get; set; }
        public List<AllowedGrantType> GrantTypes { get; set; } = new List<AllowedGrantType>();
        public List<string> Scopes { get; set; } = new List<string>();
        public List<string> RedirectUris { get; set; } = new List<string>();
        public List<string> PostLogoutRedirectUris { get; set; } = new List<string>();
    }
}
