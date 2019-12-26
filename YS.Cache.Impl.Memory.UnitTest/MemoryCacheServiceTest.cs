using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YS.Cache.Impl.Memory
{
    [TestClass]
    public class MemoryCacheServiceTest : CacheServiceUnitTestBase
    {
        protected override ICacheService OnCreateCacheService()
        {
            var host = Knife.Hosting.Host.CreateHost();
            return host.Services.GetRequiredService<ICacheService>();
        }
    }


}
