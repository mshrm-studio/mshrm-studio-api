using Duende.IdentityServer.EntityFramework.Entities;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Auth.Domain.Clients.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Domain.Users
{
    public interface IClientFactory
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
        public Client CreateNewClient(string idName, string clientName, List<AllowedGrantType> grantTypes, List<string> scopes, string secret, List<string> redirectUris);
    }
}
