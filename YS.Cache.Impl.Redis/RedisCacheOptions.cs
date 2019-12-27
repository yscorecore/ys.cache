using System;
using System.Collections.Generic;
using System.Text;

namespace YS.Cache.Impl.Redis
{
    [OptionsClass]
    public class RedisCacheOptions
    {
        public string InstanceName { get; set; } = "Default";
        public string ConnectionString { get; set; } = "localhost";
    }
}
