using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace YS.Cache.Impl.Memory
{
    [ServiceClass]
    public class MemoryCacheService : ICacheService
    {
        public MemoryCacheService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }
        private IMemoryCache memoryCache;


        public Task<(bool, T)> Get<T>(string key)
        {
            if (this.memoryCache.TryGetValue(key, out T val))
            {
                return Task.FromResult((true, val));
            }
            return Task.FromResult((false, default(T)));
        }

        public Task RemoveByKey(string key)
        {
            this.memoryCache.Remove(key);
            return Task.CompletedTask;
        }
        public Task Set<T>(string key, T value, TimeSpan slidingTimeSpan)
        {
            this.AssertNotNull(nameof(value), value);
            var cacheOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = slidingTimeSpan,
            };

            this.memoryCache.Set(key, value, cacheOptions);
            return Task.CompletedTask;
        }
        public Task Set<T>(string key, T value, DateTimeOffset absoluteDateTimeOffset)
        {
            this.AssertNotNull(nameof(value), value);
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = absoluteDateTimeOffset,
            };
            this.memoryCache.Set(key, value, cacheOptions);
            return Task.CompletedTask;
        }
        private void AssertNotNull<T>(string name, T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
