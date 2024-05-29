using Microsoft.Extensions.Caching.Distributed;
using Mshrm.Studio.Shared.Extensions;
using Mshrm.Studio.Shared.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Services
{
    /// <summary>
    /// Make singleton so we can store all keys added
    /// </summary>
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IDictionary<string, bool> _keys;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            _keys = new ConcurrentDictionary<string, bool>();
        }

        /// <summary>
        /// Get or set and then get an item from the cache
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <param name="key">The cahce key</param>
        /// <param name="function">The place to get item from if not already in cache</param>
        /// <param name="expiresInMinutes">When the item should expire from cache</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>A item from cache</returns>
        public async Task<T> GetOrSetItemAsync<T>(string key, Func<Task<T>> function, CancellationToken cancellationToken, int expiresInMinutes)
        {
            var item = await _distributedCache.GetItemAsync<T>(key, cancellationToken);
            if(item != null)
            {
                return item;
            }

            item = await function();
            await _distributedCache.AddItemAsync(key, item, expiresInMinutes, cancellationToken);
            _keys[key] = true;

            return item;
        }

        /// <summary>
        /// Clear items that start with the key provided
        /// </summary>
        /// <param name="key">The key to find all keys that start with this</param>
        /// <returns>An async task</returns>
        public async Task ClearItemsThatStartWithAsync(string key)
        {
            // Null check
            if (string.IsNullOrEmpty(key))
                return;

            // Get all matching keys that start with the key provided
            var allKeys = _keys.Keys?.Where(x => x.ToLower().StartsWith(key.ToLower().Trim()));
            if ((allKeys?.Any() ?? false))
                return;

            foreach (var keyValue in allKeys)
            {
                // Try to get item by key
                var existingItem = await _distributedCache.GetAsync(keyValue);
                if ((existingItem?.Any() ?? false))
                {
                    // Remove item by key
                    await _distributedCache.RemoveAsync(keyValue);
                }

                // Remove the stored key since it no longer exists in cache
                _keys.Remove(keyValue);
            }

            return;
        }
    }
}
