
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Models.Enums;
using Mshrm.Studio.Shared.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Models.Pagination;
using Mshrm.Studio.Shared.Repositories.Bases;

namespace Mshrm.Studio.Domain.Domain.Tools
{
    public interface IToolRepository
    {
        /// <summary>
        /// Create a new tool
        /// </summary>
        /// <param name="name">The name of the tool</param>
        /// <param name="description">The tools description</param>
        /// <param name="link">A link to the tool</param>
        /// <param name="logoGuidId">The logos key</param>
        /// <param name="rank">The order in which is shown</param>
        /// <param name="toolType">The type of tool</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The tool</returns>
        public Task<Tool> CreateToolAsync(string name, string? description, string link, int rank, Guid logoGuidId, ToolType toolType, CancellationToken cancellationToken);

        /// <summary>
        /// Update a tool
        /// </summary>
        /// <param name="toolGuidId">The tools guid id</param>
        /// <param name="description">The new description</param>
        /// <param name="name">The new tool name</param>
        /// <param name="link">The new link</param>
        /// <param name="rank">The new rank</param>
        /// <param name="logoGuidId">The new logo guid</param>
        /// <param name="toolType">The tool type</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The updated tool</returns>
        public Task<Tool?> UpdateToolAsync(Guid toolGuidId, string name, string? description, string link, int rank, Guid? logoGuidId, ToolType toolType,
            CancellationToken cancellationToken);

        /// <summary>
        /// Get a tool
        /// </summary>
        /// <param name="guid">The guid ID for the tool</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A tool if exists</returns>
        public Task<Tool?> GetToolAsync(Guid guid, CancellationToken cancellationToken);

        /// <summary>
        /// Get a page of tools
        /// </summary>
        /// <param name="searchTerm">A search term (message)</param>
        /// <param name="name">The tools name</param>
        /// <param name="toolType">The tools type</param>
        /// <param name="page">The page</param>
        /// <param name="sortOrder">The order returned</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A page of tools</returns>
        public Task<PagedResult<Tool>> GetToolsPagedAsync(string? searchTerm, string? name, ToolType? toolType, Page page, SortOrder sortOrder, CancellationToken cancellationToken);
    }
}
