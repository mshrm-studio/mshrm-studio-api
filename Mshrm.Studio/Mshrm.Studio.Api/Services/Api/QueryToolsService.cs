using Mshrm.Studio.Api.Clients.Domain;
using Mshrm.Studio.Api.Services.Api.Interfaces;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Api.Services.Api
{
    /// <summary>
    /// Query tools
    /// </summary>
    public class QueryToolsService : IQueryToolsService
    {
        private readonly IDomainToolsClient _domainToolsClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryToolsService"/> class.
        /// </summary>
        /// <param name="domainToolsClient"></param>
        public QueryToolsService(IDomainToolsClient domainToolsClient)
        {
            _domainToolsClient = domainToolsClient;
        }

        /// <summary>
        /// Get a tool
        /// </summary>
        /// <param name="guid">The guid ID for the tool</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A tool if exists</returns>
        public async Task<ToolDto> GetToolAsync(Guid guid, CancellationToken cancellationToken)
        {
            return await _domainToolsClient.GetToolByGuidAsync(guid, cancellationToken);
        }

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
        public async Task<PageResultDtoOfToolDto> GetToolsAsync(string? searchTerm, string? name, ToolType? toolType, Page page, SortOrder sortOrder, CancellationToken cancellationToken)
        {
            return await _domainToolsClient.GetToolsAsync(searchTerm, name, toolType, sortOrder.PropertyName, (Order)sortOrder.Order, (int)page.PageNumber, (int)page.PerPage, cancellationToken);
        }
    }
}
