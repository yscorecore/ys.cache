using System;
using System.Collections.Generic;
using System.Text;

namespace YS.Cache
{
    public interface ICacheService
    {
        (bool Exists, T Value) Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan slidingTimeSpan);
        void Set<T>(string key, T value, DateTimeOffset absoluteDateTimeOffset);
        void RemoveByKey(string key);

    }
}
