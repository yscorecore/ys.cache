using System;
using System.Threading.Tasks;

namespace YS.Cache
{
    public interface ICacheService
    {
        Task<(bool Exists, T Value)> Get<T>(string key);
        Task Set<T>(string key, T value, TimeSpan slidingTimeSpan);
        Task Set<T>(string key, T value, DateTimeOffset absoluteDateTimeOffset);
        Task RemoveByKey(string key);

    }
}
