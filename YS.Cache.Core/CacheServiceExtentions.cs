using System;
using System.Threading.Tasks;

namespace YS.Cache
{
    public static class CacheServiceExtentions
    {
        public static async Task<T> TryGet<T>(this ICacheService cacheService, string key)
        {
            var (ok, val) = await cacheService.Get<T>(key);
            return ok ? val : default(T);
        }

        public static async Task<T> GetOrCreate<T>(this ICacheService cacheService, string key, Func<string, T> valueFactory, TimeSpan slidingTimeSpan)
        {
            var (ok, val) = await cacheService.Get<T>(key);
            if (ok)
            {
                return val;
            }
            else
            {
                var newValue = valueFactory(key);
                await cacheService.Set(key, newValue, slidingTimeSpan);
                return newValue;
            }
        }
        public static async Task<T> GetOrCreate<T>(this ICacheService cacheService, string key, Func<string, T> valueFactory, DateTimeOffset absoluteDateTimeOffset)
        {
            var (ok, val) = await cacheService.Get<T>(key);
            if (ok)
            {
                return val;
            }
            else
            {
                var newValue = valueFactory(key);
                await cacheService.Set(key, newValue, absoluteDateTimeOffset);
                return newValue;
            }
        }
    }
}
