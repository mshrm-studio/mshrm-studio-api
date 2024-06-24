using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mshrm.Studio.Auth.Domain.Clients;
using Mshrm.Studio.Auth.Domain.Clients.Enums;
using Mshrm.Studio.Auth.Domain.Users;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Helpers;
using Mshrm.Studio.Shared.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client = Duende.IdentityServer.EntityFramework.Entities.Client;

namespace Mshrm.Studio.Auth.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ConfigurationDbContext _configurationDbContext;
        private readonly IClientFactory _clientFactory;

        public ClientRepository(ConfigurationDbContext configurationDbContext, IClientFactory clientFactory)
        {
            _configurationDbContext = configurationDbContext;
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Create a new client
        /// </summary>
        /// <param name="idName">The clients id prefix</param>
        /// <param name="clientName">The clients name</param>
        /// <param name="grantTypes">Any grant types to support for the client</param>
        /// <param name="scopes">The scopes the client can access</param>
        /// <param name="redirectUris">The uris to redirect to</param>
        /// <returns>The new client + its secret as a tuple</returns>
        public async Task<(Client Client, string Secret)> CreateClientAsync(string idName, string clientName, List<AllowedGrantType> grantTypes, List<string> scopes, List<string> redirectUris)
        {
            // Create the secret
            var secret = StringUtility.GetRandomPassword(32);

            // Create client
            var client = _clientFactory.CreateNewClient(idName, clientName, grantTypes, scopes, secret, redirectUris);

            // Save client
            _configurationDbContext.Clients.Add(client);
            await _configurationDbContext.SaveChangesAsync();

            return (client, secret);
        }

        /// <summary>
        /// Get client by client id name
        /// </summary>
        /// <param name="idName">The id name</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A client</returns>
        public async Task<Client?> GetClientByClientIdAsync(string idName, CancellationToken cancellationToken)
        {
            return await _configurationDbContext.Clients.Where(x => x.ClientId == (idName.ToLower() + ".client"))
                .FirstOrDefaultAsync(cancellationToken);
        }

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
        public async Task<PagedResult<Client>> GetClientsPagedAsync(string? searchTerm, string? clientId, string? clientName, Page page, SortOrder sortOrder, CancellationToken cancellationToken)
        {
            var clients = _configurationDbContext.Clients.Include(x => x.ClientSecrets)
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.AllowedScopes)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                clients = clients.Where(x =>
                    x.ClientName.ToLower().Contains(searchTerm.ToLower().Trim()) ||
                    x.ClientId.ToLower().Contains(searchTerm.ToLower().Trim()) ||
                    (x.Description != null && x.Description.ToLower().Contains(searchTerm.ToLower().Trim()))
                );;
            }

            if (!string.IsNullOrEmpty(clientName))
            {
                clients = clients.Where(x => x.ClientName.ToLower().Contains(clientName.ToLower().Trim()));
            }

            if (!string.IsNullOrEmpty(clientId))
            {
                clients = clients.Where(x => x.ClientId.ToLower().Contains(clientId.ToLower().Trim()));
            }

            // Order 
            clients = OrderSet(clients, sortOrder);

            // Enumerate page
            var returnPage = await clients.PageAsync(page, cancellationToken);

            // Return as page
            return new PagedResult<Client>()
            {
                Page = page,
                SortOrder = sortOrder,
                Results = returnPage,
                TotalResults = clients.Count()
            };
        }

        #region Helpers

        /// <summary>
        /// Orders set in an enumerable list
        /// </summary>
        /// <param name="set">The list to order</param>
        /// <param name="sortOrder">The sort order details</param>
        /// <returns>Sorted list</returns>
        private IQueryable<Client> OrderSet(IQueryable<Client> set, SortOrder sortOrder)
        {
            return (sortOrder.PropertyName.Trim(), sortOrder.Order) switch
            {
                ("createdDate", Order.Ascending) => set.OrderBy(x => x.Created),
                ("createdDate", Order.Descending) => set.OrderByDescending(x => x.Created),
                ("clientName", Order.Ascending) => set.OrderBy(x => x.ClientName),
                ("clientName", Order.Descending) => set.OrderByDescending(x => x.ClientName),
                ("clientId", Order.Ascending) => set.OrderBy(x => x.ClientId),
                ("clientId", Order.Descending) => set.OrderByDescending(x => x.ClientId),
                _ => sortOrder.Order == Order.Descending ? set.OrderBy(x => x.Created) : set.OrderByDescending(x => x.Created)
            };
        }

        #endregion
    }
}
