using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
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
        [TestMethod]
        public void GetOrCreateWithSlidingTimeSpanShouldReturnCachedValueIfHasCachedValue()
        {
            var cache = Mock.Of<ICacheService>();
            Mock.Get(cache).Setup(p => p.Get<string>("abc")).Returns((true, "val"));
            var actual = cache.GetOrCreate("abc",(a)=>"bcd",TimeSpan.FromSeconds(1));
            Assert.AreEqual("val", actual);
        }
        [TestMethod]
        public void GetOrCreateWithSlidingTimeShouldGetNewValueIfNoCachedValue()
        {
            var cache = Mock.Of<ICacheService>();
            Mock.Get(cache).Setup(p => p.Get<string>("abc")).Returns((false, null));
            var actual = cache.GetOrCreate("abc", (a) => "bcd", TimeSpan.FromSeconds(1));
            Assert.AreEqual("bcd", actual);
        }
        [TestMethod]
        public void GetOrCreateWithAbsoluteDateTimeShouldReturnCachedValueIfHasCachedValue()
        {
            var cache = Mock.Of<ICacheService>();
            Mock.Get(cache).Setup(p => p.Get<string>("abc")).Returns((true, "val"));
            var actual = cache.GetOrCreate("abc", (a) => "bcd", DateTimeOffset.Now.AddSeconds(1));
            Assert.AreEqual("val", actual);
        }
        [TestMethod]
        public void GetOrCreateWithAbsoluteDateTimeShouldGetNewValueIfNoCachedValue()
        {
            var cache = Mock.Of<ICacheService>();
            Mock.Get(cache).Setup(p => p.Get<string>("abc")).Returns((false, null));
            var actual = cache.GetOrCreate("abc", (a) => "bcd", DateTimeOffset.Now.AddSeconds(1));
            Assert.AreEqual("bcd", actual);
        }
    }
}
