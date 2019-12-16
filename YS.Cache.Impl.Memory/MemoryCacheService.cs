using Microsoft.Extensions.Caching.Memory;
using System;

namespace YS.Cache.Impl.Memory
{
    public class MemoryCacheService : ICacheService
    {
        public MemoryCacheService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }
        private IMemoryCache memoryCache;


        public (bool, T) Get<T>(string key)
        {
            if (this.memoryCache.TryGetValue(key, out T val))
            {
                return (true, val);
            }
            return (false, default(T));
        }

        public void RemoveByKey(string key)
        {
            this.memoryCache.Remove(key);
        }
        public void Set<T>(string key, T value, TimeSpan cacheTimeSpan, Action<string, object> expireCallback = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheTimeSpan,
            };
            if (expireCallback != null)
            {
                cacheOptions.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration
                {
                    EvictionCallback = (k, v, reason, state) =>
                    {
                        if (reason == EvictionReason.Expired)
                        {
                            expireCallback(k as string, v);
                        }
                    }
                });
            }
            this.memoryCache.Set(key, value, cacheOptions);
        }
    }
}
