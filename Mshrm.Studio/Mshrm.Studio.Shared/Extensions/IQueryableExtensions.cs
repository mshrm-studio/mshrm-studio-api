using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Shared.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Extensions
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Get a page of items from a queryable list
        /// </summary>
        /// <typeparam name="T">The type of set to page</typeparam>
        /// <param name="set">The set to page</param>
        /// <param name="page">The page to return</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The enumerated set</returns>
        public static async Task<List<T>> PageAsync<T>(this IQueryable<T> set, Page page, CancellationToken cancellationToken)
        {
            return await set.Skip((int)(page.PageNumber * page.PerPage))
                .Take((int)page.PerPage)
                .ToListAsync(cancellationToken);
        }
    }
}
