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

namespace Mshrm.Studio.Auth.Infrastructure.Factories
{
    public class ApiResourceFactory : IApiResourceFactory
    {
        /// <summary>
        /// Create a new api resource
        /// </summary>
        /// <param name="name">The new resources name</param>
        /// <param name="scopes">The resource scopes</param>
        /// <param name="secret">The secret hashed</param>
        /// <returns>A new api resource</returns>
        public ApiResource CreateNewApiResource(string name, List<string> scopes, string secret)
        {
            return new ApiResource
            {
                Name = name,
                Scopes = scopes.Select(x => new ApiResourceScope() { Scope = x.ToLower().Trim() }).ToList(),
                Secrets = new List<ApiResourceSecret>(){ new ApiResourceSecret() { Value = secret.Sha256() } },
                UserClaims = new List<ApiResourceClaim>() { new ApiResourceClaim() { Type = "role" } },
            };
        }
    }
}
