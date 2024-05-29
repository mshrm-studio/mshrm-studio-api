using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Domain.Api.Context;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Domain.Api.Models.Enums;
using Mshrm.Studio.Domain.Api.Repositories.Interfaces;
using Mshrm.Studio.Domain.Domain.Tools;
using Mshrm.Studio.Shared.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Models.Pagination;
using Mshrm.Studio.Shared.Repositories.Bases;

namespace Mshrm.Studio.Domain.Api.Repositories
{
    public class ToolRepository : BaseRepository<Tool, MshrmStudioDomainDbContext>, IToolRepository
    {
        private readonly IToolFactory _toolFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public ToolRepository(MshrmStudioDomainDbContext context, IToolFactory toolFactory) : base(context)
        {
            _toolFactory = toolFactory;
        }

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
        public async Task<Tool> CreateToolAsync(string name, string? description, string link, int rank, Guid logoGuidId, ToolType toolType, CancellationToken cancellationToken)
        {
            var tool = _toolFactory.CreateTool(name, description, link, rank, logoGuidId, toolType);

            Add(tool);
            await SaveAsync(cancellationToken);

            return tool;
        }

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
        public async Task<Tool?> UpdateToolAsync(Guid toolGuidId, string name, string? description, string link, int rank, Guid? logoGuidId, ToolType toolType,
            CancellationToken cancellationToken)
        {
            var tool = GetAll()
                .AsNoTracking()
                .FirstOrDefault(x => x.GuidId == toolGuidId);
            if (tool == null)
                return null;

            tool.SetName(name);
            tool.SetDescription(description);
            tool.SetLink(link);
            tool.SetRank(rank);
            tool.SetToolType(toolType);

            if (logoGuidId.HasValue)
                tool.SetLogoGuidId(logoGuidId.Value);

            Update(tool);
            await SaveAsync(cancellationToken);

            return tool;
        }

        /// <summary>
        /// Get a tool
        /// </summary>
        /// <param name="guid">The guid ID for the tool</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A tool if exists</returns>
        public async Task<Tool?> GetToolAsync(Guid guid, CancellationToken cancellationToken)
        {
            return await GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.GuidId == guid, cancellationToken);
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
        public async Task<PagedResult<Tool>> GetToolsPagedAsync(string? searchTerm, string? name, ToolType? toolType, Page page, SortOrder sortOrder, CancellationToken cancellationToken)
        {
            var tools = GetAll()
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                tools = tools.Where(x =>
                    x.Name.ToLower().Contains(searchTerm.ToLower().Trim()) ||
                    (x.Description != null && x.Description.ToLower().Contains(searchTerm.ToLower().Trim()))
                );
            }

            if (!string.IsNullOrEmpty(name))
            {
                tools = tools.Where(x => x.Name.ToLower().Contains(name.ToLower().Trim()));
            }

            if (toolType.HasValue)
            {
                tools = tools.Where(x => x.ToolType == toolType);
            }

            // Order 
            tools = OrderSet(tools, sortOrder);

            // Enumerate page
            var returnPage = await tools.PageAsync(page, cancellationToken);

            // Return as page
            return new PagedResult<Tool>()
            {
                Page = page,
                SortOrder = sortOrder,
                Results = returnPage,
                TotalResults = tools.Count()
            };
        }

        #region Helpers

        /// <summary>
        /// Orders set in an enumerable list
        /// </summary>
        /// <param name="set">The list to order</param>
        /// <param name="sortOrder">The sort order details</param>
        /// <returns>Sorted list</returns>
        private IQueryable<Tool> OrderSet(IQueryable<Tool> set, SortOrder sortOrder)
        {
            return (sortOrder.PropertyName.Trim(), sortOrder.Order) switch
            {
                ("createdDate", Order.Ascending) => set.OrderBy(x => x.CreatedDate),
                ("createdDate", Order.Descending) => set.OrderByDescending(x => x.CreatedDate),
                ("rank", Order.Ascending) => set.OrderBy(x => x.Rank),
                ("rank", Order.Descending) => set.OrderByDescending(x => x.Rank),
                _ => sortOrder.Order == Order.Descending ? set.OrderBy(x => x.Rank) : set.OrderByDescending(x => x.Rank)
            };
        }

        #endregion
    }
}
