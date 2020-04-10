using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace YS.Cache.Impl.Redis
{
    [ServiceClass(Lifetime = ServiceLifetime.Singleton)]
    public class RedisCacheService : ICacheService
    {
        public RedisCacheService(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }
        private IDistributedCache distributedCache;

        public async Task<(bool, T)> Get<T>(string key)
        {
            if (typeof(T) == typeof(string))
            {
                var str = await this.distributedCache.GetStringAsync(key);
                return (str != null, (T)(object)str);
            }
            else
            {
                var bys = await this.distributedCache.GetAsync(key);
                if (bys == null)
                {
                    return (false, default(T));
                }
                else
                {
                    return (true, JsonSerializer.Deserialize<T>(bys));
                }
            }
        }

        public Task RemoveByKey(string key)
        {
            return this.distributedCache.RemoveAsync(key);
        }

        public Task Set<T>(string key, T value, TimeSpan slidingTimeSpan)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (value is string)
            {
                return this.distributedCache.SetStringAsync(key,
                    value as string,
                    new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = slidingTimeSpan,
                    });
            }
            else
            {
                return this.distributedCache.SetAsync(key,
                    JsonSerializer.SerializeToUtf8Bytes(value),
                    new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = slidingTimeSpan,
                    });
            }
        }
        public Task Set<T>(string key, T value, DateTimeOffset absoluteDateTimeOffset)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (value is string)
            {
                return this.distributedCache.SetStringAsync(key, value as string, new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = absoluteDateTimeOffset
                });
            }
            else
            {
                return this.distributedCache.SetAsync(key,
                    JsonSerializer.SerializeToUtf8Bytes(value),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = absoluteDateTimeOffset,
                    });
            }
        }
    }
}
