using Duende.IdentityServer.EntityFramework.Entities;
using Mshrm.Studio.Auth.Domain.Clients.Enums;
using Mshrm.Studio.Shared.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Domain.Clients
{
    public interface IClientRepository
    {
        /// <summary>
        /// Create a new client
        /// </summary>
        /// <param name="idName">The clients id prefix</param>
        /// <param name="clientName">The clients name</param>
        /// <param name="grantTypes">Any grant types to support for the client</param>
        /// <param name="scopes">The scopes the client can access</param>
        /// <param name="redirectUris">The uris to redirect to</param>
        /// <returns>The new client + its secret as a tuple</returns>
        public Task<(Client Client, string Secret)> CreateClientAsync(string idName, string clientName, List<AllowedGrantType> grantTypes, List<string> scopes, List<string> redirectUris);

        /// <summary>
        /// Get client by client id name
        /// </summary>
        /// <param name="idName">The id name</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A client</returns>
        Task<Client?> GetClientByClientIdAsync(string idName, CancellationToken cancellationToken);

        /// <summary>
        /// Get a page of clients
        /// </summary>
        /// <param name="searchTerm">A search term</param>
        /// <param name="clientId">A client id</param>
        /// <param name="clientName">A client name</param>
        /// <param name="page">The page to return</param>
        /// <param name="sortOrder">The order to return</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A page of clients</returns>
        Task<PagedResult<Client>> GetClientsPagedAsync(string? searchTerm, string? clientId, string? clientName, Page page, SortOrder sortOrder, CancellationToken cancellationToken);
    }
}
