using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
namespace YS.Cache
{
    [TestClass]
    public class CacheServiceExtentionsTest
    {
        [TestMethod]
        public void TryGetNotContainsKeyShouldReturnDefaultValue()
        {
            var cache = Mock.Of<ICacheService>();
            var actual = cache.TryGet<string>("abc");
            Assert.AreEqual(default(string), actual);
        }
        [TestMethod]
        public void TryGetContainsKeyShouldReturnCachedValue()
        {
            var cache = Mock.Of<ICacheService>();
            Mock.Get(cache).Setup(p => p.Get<string>("abc")).Returns((true, "val"));
            var actual = cache.TryGet<string>("abc");
            Assert.AreEqual("val", actual);
        }

    }
}
