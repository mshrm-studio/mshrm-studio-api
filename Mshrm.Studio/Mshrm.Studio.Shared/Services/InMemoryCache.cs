using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Services
{
    public class InMemoryCache : IDistributedCache
    {
        private readonly IMemoryCache _cache;

        public InMemoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public byte[]? Get(string key)
        {
            _cache.TryGetValue(key, out var value);

            return (byte[])value;
        }

        public Task<byte[]?> GetAsync(string key, CancellationToken token = default)
        {
            _cache.TryGetValue(key, out var value);

            return Task.FromResult((byte[])value);
        }

        public void Refresh(string key)
        {
            throw new NotImplementedException();
        }

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            _cache.Remove(key);

            return Task.FromResult(0);
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            _cache.Set(key, value);
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            _cache.Set(key, value);

            return Task.FromResult(0);
        }
    }
}
