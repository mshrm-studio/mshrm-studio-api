using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Auth.Domain.ApiResources;
using Mshrm.Studio.Auth.Domain.Users;
using Mshrm.Studio.Auth.Infrastructure.Factories;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Helpers;
using Mshrm.Studio.Shared.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mshrm.Studio.Auth.Infrastructure.Repositories
{
    public class ApiScopeRepository : IApiScopeRepository
    {
        private readonly ConfigurationDbContext _configurationDbContext;
        private readonly IApiScopeFactory _apiScopeFactory;

        public ApiScopeRepository(ConfigurationDbContext configurationDbContext, IApiScopeFactory apiScopeFactory)
        {
            _configurationDbContext = configurationDbContext;
            _apiScopeFactory = apiScopeFactory;
        }

        /// <summary>
        /// Create a new api scope
        /// </summary>
        /// <param name="name">The api scope name</param>
        /// <returns>The new api scope</returns>
        public async Task<ApiScope> CreateApiScopeAsync(string name, CancellationToken cancellationToken)
        {
            // Create api resource
            var apiScope = _apiScopeFactory.CreateNewApiScope(name);
            _configurationDbContext.ApiScopes.Add(apiScope);

            // Save api scope
            await _configurationDbContext.SaveChangesAsync(cancellationToken);

            return apiScope;
        }

        /// <summary>
        /// Get an api resource by id
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>An api resource</returns>
        public async Task<ApiScope?> GetApiScopeByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _configurationDbContext.ApiScopes.Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Get an api resource by name
        /// </summary>
        /// <param name="id">The name</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>An api resource</returns>
        public async Task<ApiScope?> GetApiScopeByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _configurationDbContext.ApiScopes.Where(x => x.Name.ToLower().Trim() == name.ToLower().Trim())
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Get a page of api resources
        /// </summary>
        /// <param name="searchTerm">A search term</param>
        /// <param name="name">A name</param>
        /// <param name="page">The page to return</param>
        /// <param name="sortOrder">The order to return</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A page of api resources</returns>
        public async Task<PagedResult<ApiScope>> GetApiScopesPagedAsync(string? searchTerm, string? name, Page page, SortOrder sortOrder, CancellationToken cancellationToken)
        {
            var apiResources = _configurationDbContext.ApiScopes
                .Include(x => x.UserClaims)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                apiResources = apiResources.Where(x =>
                    x.Name.ToLower().Contains(searchTerm.ToLower().Trim()) ||
                    (x.Description != null && x.Description.ToLower().Contains(searchTerm.ToLower().Trim()))
                ); ;
            }

            if (!string.IsNullOrEmpty(name))
            {
                apiResources = apiResources.Where(x => x.Name.ToLower().Contains(name.ToLower().Trim()));
            }

            // Order 
            apiResources = OrderSet(apiResources, sortOrder);

            // Enumerate page
            var returnPage = await apiResources.PageAsync(page, cancellationToken);

            // Return as page
            return new PagedResult<ApiScope>()
            {
                Page = page,
                SortOrder = sortOrder,
                Results = returnPage,
                TotalResults = apiResources.Count()
            };
        }

        #region Helpers

        /// <summary>
        /// Orders set in an enumerable list
        /// </summary>
        /// <param name="set">The list to order</param>
        /// <param name="sortOrder">The sort order details</param>
        /// <returns>Sorted list</returns>
        private IQueryable<ApiScope> OrderSet(IQueryable<ApiScope> set, SortOrder sortOrder)
        {
            return (sortOrder.PropertyName.Trim(), sortOrder.Order) switch
            {
                ("createdDate", Order.Ascending) => set.OrderBy(x => x.Created),
                ("createdDate", Order.Descending) => set.OrderByDescending(x => x.Created),
                ("name", Order.Ascending) => set.OrderBy(x => x.Name),
                ("name", Order.Descending) => set.OrderByDescending(x => x.Name),
                _ => sortOrder.Order == Order.Descending ? set.OrderBy(x => x.Created) : set.OrderByDescending(x => x.Created)
            };
        }

        #endregion
    }
}
