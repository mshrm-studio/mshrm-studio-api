
using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Api.Services.Api.Interfaces
{
    public interface IQueryToolsService
    {
        /// <summary>
        /// Get a tool
        /// </summary>
        /// <param name="guid">The guid ID for the tool</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A tool if exists</returns>
        Task<ToolDto> GetToolAsync(Guid guid, CancellationToken cancellationToken);

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
        public Task<PageResultDtoOfToolDto> GetToolsAsync(string? searchTerm, string? name, ToolType? toolType, Page page, SortOrder sortOrder, CancellationToken cancellationToken);
    }
}
