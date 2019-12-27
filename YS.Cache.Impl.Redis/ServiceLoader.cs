using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace YS.Cache.Impl.Redis
{
    public class ServiceLoader : IServiceLoader
    {
        public void LoadServices(IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetConfigOrNew<RedisCacheOptions>();
            services.AddDistributedRedisCache((setupAction) =>
            {
                setupAction.InstanceName = options.InstanceName;
                setupAction.Configuration = options.ConnectionString;
            });
        }
    }
}
