using Duende.IdentityServer.EntityFramework.Entities;
using Mshrm.Studio.Auth.Domain.Clients.Enums;
using Mshrm.Studio.Shared.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Auth.Domain.ApiResources
{
    public interface IApiScopeRepository
    {
        /// <summary>
        /// Create a new api scope
        /// </summary>
        /// <param name="name">The api scope name</param>
        /// <returns>The new api scope + its secret as a tuple</returns>
        public Task<ApiScope> CreateApiScopeAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Get an api scope by id
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>An api scope</returns>
        Task<ApiScope?> GetApiScopeByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Get an api scope by name
        /// </summary>
        /// <param name="id">The name</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>An api scope</returns>
        Task<ApiScope?> GetApiScopeByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Get a page of api scope
        /// </summary>
        /// <param name="searchTerm">A search term</param>
        /// <param name="name">A name</param>
        /// <param name="page">The page to return</param>
        /// <param name="sortOrder">The order to return</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A page of api scopes</returns>
        Task<PagedResult<ApiScope>> GetApiScopesPagedAsync(string? searchTerm, string? name, Page page, SortOrder sortOrder, CancellationToken cancellationToken);
    }
}
