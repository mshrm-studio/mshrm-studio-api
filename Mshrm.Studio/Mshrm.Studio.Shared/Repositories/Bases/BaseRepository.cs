using Microsoft.EntityFrameworkCore;
using Mshrm.Studio.Shared.Api.Repositories.Interfaces;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Models.Entities.Interfaces;

namespace Mshrm.Studio.Shared.Repositories.Bases
{
    /// <summary>
    /// The base functionality of a repository - implements an interface
    /// </summary>
    /// <typeparam name="TEntity">The type of repository</typeparam>
    /// <typeparam name="TDbContext">The context (X:DbContext)</typeparam>
    public abstract class BaseRepository<TEntity, TDbContext> : IRepository<TEntity>
       where TDbContext : DbContext where TEntity : IAggregateRoot
    {
        /// <summary>
        /// Database context
        /// </summary>
        protected readonly TDbContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The database context</param>
        public BaseRepository(TDbContext context)
        {
            // Setup the context
            _context = context;
        }

        /// <summary>
        /// Add an item to the context
        /// </summary>
        /// <param name="item">Item to add to context</param>
        /// <returns>Added item to context</returns>
        public TEntity Add(TEntity item)
        {
            _context.Add(item);
            return item;
        }

        /// <summary>
        /// Add items to the context
        /// </summary>
        /// <param name="item">Items to add to context</param>
        /// <returns>Added items to context</returns>
        public List<TEntity> AddRange(List<TEntity> items)
        {
            if ((items?.Any() ?? false))
            {
                _context.AddRange(items);
            }

            return items;
        }

        /// <summary>
        /// Remove items to the context
        /// </summary>
        /// <param name="item">Items to remove to context</param>
        /// <returns>Removed items to context</returns>
        public List<TEntity> RemoveRange(List<TEntity> items)
        {
            if ((items?.Any() ?? false))
            {
                _context.RemoveRange(items);
            }

            return items;
        }

        /// <summary>
        /// Removes a tracked item in the context
        /// </summary>
        /// <param name="item">The item to remove</param>
        public void Remove(TEntity item)
        {
            _context.Remove(item);
        }

        /// <summary>
        /// Gets all items from context - is overrideable
        /// </summary>
        /// <param name="tableName">Optional tablename param</param>
        /// <returns>List of items</returns>
        public virtual IQueryable<TEntity> GetAll(string? tableName = null)
        {
            return _context.GetPropertyValue(tableName ?? $"{typeof(TEntity).Name}s");
        }

        /// <summary>
        /// Updates a tracked item in the context
        /// </summary>
        /// <param name="item">The item to update</param>
        /// <returns>The updated item</returns>
        public TEntity Update(TEntity item)
        {
            _context.Attach(item);
            _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return item;
        }

        /// <summary>
        /// Updates items in context
        /// </summary>
        /// <param name="items">Items to update in context</param>
        /// <returns>Updated items in context</returns>
        public IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> items)
        {
            foreach (var item in items)
            {
                _context.Attach(item);
                _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

            return items;
        }

        /// <summary>
        /// Saves the context
        /// </summary>
        /// <returns>The number of records inserted/removed/updated</returns>
        public int Save()
        {
            return _context.SaveChanges();
        }

        /// <summary>
        /// Saves the context
        /// </summary>
        /// <returns>The number of records inserted/removed/updated</returns>
        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
