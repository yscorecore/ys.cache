using System;
using System.Collections.Generic;
using System.Text;

namespace YS.Cache
{
    public static class CacheServiceExtentions
    {
        public static T TryGet<T>(this ICacheService cacheService, string key)
        {
            var (ok, val) = cacheService.Get<T>(key);
            return ok ? val : default(T);
        }

        public static T GetOrCreate<T>(this ICacheService cacheService, string key, Func<string, T> valueFactory, TimeSpan slidingTimeSpan)
        {
            var (ok, val) = cacheService.Get<T>(key);
            if (ok)
            {
                return val;
            }
            else
            {
                var newValue = valueFactory(key);
                cacheService.Set(key, newValue, slidingTimeSpan);
                return newValue;
            }
        }
        public static T GetOrCreate<T>(this ICacheService cacheService, string key, Func<string, T> valueFactory, DateTimeOffset absoluteDateTimeOffset)
        {
            var (ok, val) = cacheService.Get<T>(key);
            if (ok)
            {
                return val;
            }
            else
            {
                var newValue = valueFactory(key);
                cacheService.Set(key, newValue, absoluteDateTimeOffset);
                return newValue;
            }
        }
    }
}
