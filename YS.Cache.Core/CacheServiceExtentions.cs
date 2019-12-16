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
        public static void Set<T>(this ICacheService cacheService, string key, T value, DateTimeOffset expireTime, Action<string, object> expireCallback = null)
        {
            if (expireTime < DateTimeOffset.Now)
            {
                throw new ArgumentOutOfRangeException(nameof(expireTime), "The expire time should late than current time.");
            }

            cacheService.Set(key, value, expireTime - DateTimeOffset.Now, expireCallback);

        }
        public static T GetOrCreate<T>(this ICacheService cacheService, string key, Func<string, T> valueFactory, TimeSpan cacheTimeSpan, Action<string, object> expireCallback = null)
        {
            var (ok, val) = cacheService.Get<T>(key);
            if (ok)
            {
                return val;
            }
            else
            {
                var newValue = valueFactory(key);
                cacheService.Set(key, newValue, cacheTimeSpan, expireCallback);
                return newValue;
            }
        }
        public static T GetOrCreate<T>(this ICacheService cacheService, string key, Func<string, T> valueFactory, DateTimeOffset expireTime, Action<string, object> expireCallback = null)
        {
            var (ok, val) = cacheService.Get<T>(key);
            if (ok)
            {
                return val;
            }
            else
            {
                var newValue = valueFactory(key);
                cacheService.Set(key, newValue, expireTime, expireCallback);
                return newValue;
            }
        }
    }
}
