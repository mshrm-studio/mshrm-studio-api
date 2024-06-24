using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.Models;
using IdentityServer4.Models;
using Mshrm.Studio.Auth.Domain.Clients.Enums;
using Mshrm.Studio.Auth.Domain.Users;
using Mshrm.Studio.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client = Duende.IdentityServer.EntityFramework.Entities.Client;
using GrantType = Duende.IdentityServer.Models.GrantType;

namespace Mshrm.Studio.Auth.Infrastructure.Factories
{
    public class ClientFactory : IClientFactory
    {
        /// <summary>
        /// Create a new client
        /// </summary>
        /// <param name="idName">The id prefix of the new client</param>
        /// <param name="clientName">The new clients name</param>
        /// <param name="grantTypes">Grant types for the new client</param>
        /// <param name="scopes">Scopes for the new client</param>
        /// <param name="redirectUris">The uris to redirect to</param>
        /// <returns>A new client</returns>
        public Client CreateNewClient(string idName, string clientName, List<AllowedGrantType> grantTypes, List<string> scopes, string secret, List<string> redirectUris, List<string> postLogoutRedirectUris)
        {
            return new Client
            {
                ClientId = $"{idName.ToLower()}.client",
                ClientName = $"{clientName}",
                AllowedGrantTypes = grantTypes.Select(x => new ClientGrantType()
                {
                    GrantType = (x == AllowedGrantType.ClientCredentials) ? GrantType.ClientCredentials :
                                (x == AllowedGrantType.ResourceOwnerPassword) ? GrantType.ResourceOwnerPassword :
                                (x == AllowedGrantType.AuthorizationCode) ? GrantType.AuthorizationCode :
                                (x == AllowedGrantType.Hybrid) ? GrantType.Hybrid : null
                }).ToList(),
                ClientSecrets = new List<ClientSecret> { new ClientSecret() { Value = secret.Sha256() } },
                AllowedScopes = scopes.Select(x => new ClientScope() { Scope = x }).ToList(),
                RedirectUris = redirectUris.Select(x => new ClientRedirectUri() { RedirectUri = x }).ToList(),
                PostLogoutRedirectUris = postLogoutRedirectUris.Select(x => new ClientPostLogoutRedirectUri() { PostLogoutRedirectUri = x }).ToList(),
                AllowOfflineAccess = true,
            };
        }
    }
}
