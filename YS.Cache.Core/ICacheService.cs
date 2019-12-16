using System;
using System.Collections.Generic;
using System.Text;

namespace YS.Cache
{
    public interface ICacheService
    {
        (bool, T) Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan cacheTimeSpan, Action<string, object> expireCallback = null);
        void RemoveByKey(string key);

    }
}
