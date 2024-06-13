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
    public interface IApiScopeFactory
    {
        /// <summary>
        /// Create a new api resource
        /// </summary>
        /// <param name="name">The new resources name</param>
        /// <returns>A new api resource</returns>
        public ApiScope CreateNewApiScope(string name);
    }
}
