using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace Mshrm.Studio.Shared.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task<T?> GetItemAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken cancellationToken)
        {
            // Get the bytes
            var bytes = await distributedCache.GetAsync(key, cancellationToken);

            if ((bytes?.Any() ?? false))
            {
                var item = Encoding.UTF8.GetString(bytes);

                return JsonConvert.DeserializeObject<T>(item);
            }

            return default(T);
        }

        public static async Task AddItemAsync<T>(this IDistributedCache distributedCache, string key, T item, int expiresInMinutes, CancellationToken cancellationToken)
        {
            var serializedItem = JsonConvert.SerializeObject(item, Formatting.Indented);
            var itemBytes = Encoding.UTF8.GetBytes(serializedItem);

            // Set
            await distributedCache.SetAsync(key, itemBytes, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expiresInMinutes)
            }, cancellationToken);

            return;
        }
    }
}
