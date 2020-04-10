using System;
using Microsoft.Extensions.DependencyInjection;
using YS.Knife;

namespace YS.Cache.Impl.Redis
{
    public class ServiceRegister : IServiceRegister
    {
        public void RegisteServices(IServiceCollection services, IRegisteContext context)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));
            var options = context.Configuration.GetConfigOrNew<RedisCacheOptions>();
            services.AddDistributedRedisCache((setupAction) =>
            {
                setupAction.InstanceName = options.CacheKeyPrefix;
                setupAction.Configuration = options.ConnectionString;
                setupAction.ConfigurationOptions = options.Configuration;
            });
        }
    }
}
