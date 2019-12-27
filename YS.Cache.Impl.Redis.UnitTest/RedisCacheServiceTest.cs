using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YS.Cache.Impl.Redis.UnitTest
{
    [TestClass]
    public class RedisCacheServiceTest : CacheServiceUnitTestBase
    {
        protected override ICacheService OnCreateCacheService()
        {
            var host = Knife.Hosting.Host.CreateHost();
            return host.Services.GetRequiredService<ICacheService>();
        }
    }
}
