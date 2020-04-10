using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace YS.Cache.Impl.Memory
{
    public class ServiceLoader : IServiceLoader
    {
        public void LoadServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
        }
    }
}
