using System;
using System.Collections.Generic;
using System.Text;
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
            });
        }
    }
}
