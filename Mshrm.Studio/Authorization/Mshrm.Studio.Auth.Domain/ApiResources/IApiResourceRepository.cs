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
    public interface IApiResourceRepository
    {
        /// <summary>
        /// Create a new api resource
        /// </summary>
        /// <param name="name">The api resource name</param>
        /// <returns>The new api resource + its secret as a tuple</returns>
        public Task<(ApiResource Client, string Secret)> CreateApiResourceAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Get an api resource by id
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>An api resource</returns>
        Task<ApiResource?> GetApiResourceByIdAsync(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Get an api resource by name
        /// </summary>
        /// <param name="id">The name</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>An api resource</returns>
        Task<ApiResource?> GetApiResourceByNameAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Get a page of api resources
        /// </summary>
        /// <param name="searchTerm">A search term</param>
        /// <param name="name">A name</param>
        /// <param name="page">The page to return</param>
        /// <param name="sortOrder">The order to return</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A page of api resources</returns>
        Task<PagedResult<ApiResource>> GetApiResourcesPagedAsync(string? searchTerm, string? name, Page page, SortOrder sortOrder, CancellationToken cancellationToken);
    }
}
