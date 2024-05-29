using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Services.Interfaces
{
    public interface ICacheService
    {
        /// <summary>
        /// Clear items that start with the key provided
        /// </summary>
        /// <param name="key">The key to find all keys that start with this</param>
        /// <returns>An async task</returns>
        Task ClearItemsThatStartWithAsync(string key);

        /// <summary>
        /// Get or set and then get an item from the cache
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <param name="key">The cahce key</param>
        /// <param name="function">The place to get item from if not already in cache</param>
        /// <param name="expiresInMinutes">When the item should expire from cache</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A item from cache</returns>
        public Task<T> GetOrSetItemAsync<T>(string key, Func<Task<T>> function, CancellationToken cancellationToken, int expiresInMinutes);
    }
}
