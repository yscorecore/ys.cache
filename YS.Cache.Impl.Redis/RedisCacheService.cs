using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace YS.Cache.Impl.Redis
{
    [ServiceClass]
    public class RedisCacheService : ICacheService
    {
        public RedisCacheService(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }
        private IDistributedCache distributedCache;

        public (bool, T) Get<T>(string key)
        {
            if (typeof(T) == typeof(string))
            {
                var str = this.distributedCache.GetString(key);
                return (str != null, (T)(object)str);
            }
            else
            {
                var bys = this.distributedCache.Get(key);
                if (bys == null)
                {
                    return (false, default(T));
                }
                else
                {
                    return (true, (T)Bytes2Object(bys));
                }
            }
        }

        public void RemoveByKey(string key)
        {
            this.distributedCache.Remove(key);
        }

        public void Set<T>(string key, T value, TimeSpan slidingTimeSpan)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (value is string)
            {
                this.distributedCache.SetString(key, value as string, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = slidingTimeSpan,
                });
            }
            else
            {
                this.distributedCache.Set(key, Object2Bytes(value), new DistributedCacheEntryOptions
                {
                    SlidingExpiration = slidingTimeSpan,
                });
            }
        }
        public void Set<T>(string key, T value, DateTimeOffset absoluteDateTimeOffset)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (value is string)
            {
                this.distributedCache.SetString(key, value as string, new DistributedCacheEntryOptions
                {
                     AbsoluteExpiration=absoluteDateTimeOffset
                });
            }
            else
            {
                this.distributedCache.Set(key, Object2Bytes(value), new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = absoluteDateTimeOffset,
                });
            }
        }
        private byte[] Object2Bytes(object obj)
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                return ms.ToArray();
            }
        }
        private object Bytes2Object(byte[] bys)
        {
            using (var ms = new MemoryStream(bys))
            {
                var bf = new BinaryFormatter();
                return bf.Deserialize(ms);
            }
        }

      

      
    }
}
