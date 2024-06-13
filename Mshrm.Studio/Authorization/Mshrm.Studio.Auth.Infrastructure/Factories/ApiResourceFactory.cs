using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.Models;
using Mshrm.Studio.Auth.Domain.Clients.Enums;
using Mshrm.Studio.Auth.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ApiResource = Duende.IdentityServer.EntityFramework.Entities.ApiResource;
using ApiScope = Duende.IdentityServer.EntityFramework.Entities.ApiScope;

namespace Mshrm.Studio.Auth.Infrastructure.Factories
{
    public class ApiResourceFactory : IApiScopeFactory
    {
        /// <summary>
        /// Create a new api scope
        /// </summary>
        /// <param name="name">The new scopes name</param>
        /// <returns>A new api scope</returns>
        public ApiScope CreateNewApiScope(string name)
        {
            return new ApiScope
            {
                Name = name.ToLower(),
                DisplayName = name,
                UserClaims = new List<ApiScopeClaim>() { new ApiScopeClaim() { Type = "role" } },
            };
        }
    }
}
