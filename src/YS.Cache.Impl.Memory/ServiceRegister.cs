using Microsoft.Extensions.DependencyInjection;
using YS.Knife;

namespace YS.Cache.Impl.Memory
{
    public class ServiceRegister : IServiceRegister
    {
        public void RegisteServices(IServiceCollection services, IRegisteContext context)
        {
            services.AddMemoryCache();
        }
    }
}
