using StackExchange.Redis;
using YS.Knife;

namespace YS.Cache.Impl.Redis
{
    [OptionsClass("Redis")]
    public class RedisCacheOptions
    {
        public string CacheKeyPrefix { get; set; } = "Cache_";
        public string ConnectionString { get; set; } = "localhost";
        public ConfigurationOptions Configuration { get; set; }
    }
}
