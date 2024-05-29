using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Shared.Models.Entities.Interfaces;

namespace Mshrm.Studio.Shared.Api.Repositories.Interfaces
{
    /// <summary>
    /// Interface of a repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : IAggregateRoot
    {
        /// <summary>
        /// Add an item
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <returns>Added item (with updated id)</returns>
        TEntity Add(TEntity item);

        /// <summary>
        /// Add items
        /// </summary>
        /// <param name="item">Items to add</param>
        /// <returns>Added items (with updated id)</returns>
        List<TEntity> AddRange(List<TEntity> items);

        /// <summary>
        /// Update an item
        /// </summary>
        /// <param name="item">Item to update (must be trackd)</param>
        /// <returns>Updated item</returns>
        TEntity Update(TEntity item);

        /// <summary>
        /// Update items
        /// </summary>
        /// <param name="items">Items to update (must be trackd)</param>
        /// <returns>Updated items</returns>
        IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> item);

        /// <summary>
        /// Gets all items
        /// </summary>
        /// <param name="tableName">Optional tablename param</param>
        /// <returns>List of items</returns>
        abstract IQueryable<TEntity> GetAll(string tableName = null);

        /// <summary>
        /// Save context
        /// </summary>
        /// <returns>Items updatd/inserted/removed in save</returns>
        int Save();

        /// <summary>
        /// Save context
        /// </summary>
        /// <returns>Items updatd/inserted/removed in save</returns>
        Task<int> SaveAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Remove items to the context
        /// </summary>
        /// <param name="item">Items to remove to context</param>
        /// <returns>Removed items to context</returns>
        List<TEntity> RemoveRange(List<TEntity> items);

        /// <summary>
        /// Removes a tracked item in the context
        /// </summary>
        /// <param name="item">The item to remove</param>
        void Remove(TEntity item);
    }
}
