using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
namespace YS.Cache
{
    [TestClass]
    public class CacheServiceExtentionsTest
    {
        [TestMethod]
        public async Task TryGetNotContainsKeyShouldReturnDefaultValue()
        {
            var cache = Mock.Of<ICacheService>();
            var actual = await cache.TryGet<string>("abc");
            Assert.AreEqual(default(string), actual);
        }
        [TestMethod]
        public async Task TryGetContainsKeyShouldReturnCachedValue()
        {
            var cache = Mock.Of<ICacheService>();
            Mock.Get(cache).Setup(p => p.Get<string>("abc")).Returns(Task.FromResult((true, "val")));
            var actual = await cache.TryGet<string>("abc");
            Assert.AreEqual("val", actual);
        }
        [TestMethod]
        public async Task GetOrCreateWithSlidingTimeSpanShouldReturnCachedValueIfHasCachedValue()
        {
            var cache = Mock.Of<ICacheService>();
            Mock.Get(cache).Setup(p => p.Get<string>("abc")).Returns(Task.FromResult((true, "val")));
            var actual = await cache.GetOrCreate("abc", (a) => "bcd", TimeSpan.FromSeconds(1));
            Assert.AreEqual("val", actual);
        }
        [TestMethod]
        public async Task GetOrCreateWithSlidingTimeShouldGetNewValueIfNoCachedValue()
        {
            var cache = Mock.Of<ICacheService>();
            Mock.Get(cache).Setup(p => p.Get<string>("abc")).Returns(Task.FromResult((false, default(string))));
            var actual = await cache.GetOrCreate("abc", (a) => "bcd", TimeSpan.FromSeconds(1));
            Assert.AreEqual("bcd", actual);
        }
        [TestMethod]
        public async Task GetOrCreateWithAbsoluteDateTimeShouldReturnCachedValueIfHasCachedValue()
        {
            var cache = Mock.Of<ICacheService>();
            Mock.Get(cache).Setup(p => p.Get<string>("abc")).Returns(Task.FromResult((true, "val")));
            var actual = await cache.GetOrCreate("abc", (a) => "bcd", DateTimeOffset.Now.AddSeconds(1));
            Assert.AreEqual("val", actual);
        }
        [TestMethod]
        public async Task GetOrCreateWithAbsoluteDateTimeShouldGetNewValueIfNoCachedValue()
        {
            var cache = Mock.Of<ICacheService>();
            Mock.Get(cache).Setup(p => p.Get<string>("abc")).Returns(Task.FromResult((false, default(string))));
            var actual = await cache.GetOrCreate("abc", (a) => "bcd", DateTimeOffset.Now.AddSeconds(1));
            Assert.AreEqual("bcd", actual);
        }
    }
}
