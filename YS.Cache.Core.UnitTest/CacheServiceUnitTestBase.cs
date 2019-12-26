using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace YS.Cache
{
    public abstract class CacheServiceUnitTestBase
    {
        private ICacheService cacheService;
        [TestInitialize]
        public virtual void Setup()
        {
            this.cacheService = this.OnCreateCacheService();
        }
        protected abstract ICacheService OnCreateCacheService();
        [TestMethod]
        public void ShouldReturnDefaultValueIfNotContainsKey()
        {
            var res = cacheService.Get<string>("abc");
            Assert.AreEqual(false, res.Exists);
            Assert.AreEqual(default, res.Value);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowArgumentNullExceptionIfSetNullValueWithSlidingTimeSpan()
        {
            cacheService.Set<string>("abc", null, TimeSpan.FromSeconds(5));
        }
        [TestMethod]
        public void ShouldReturnCachedValueIfAlreadySetValueWithSlidingTimeSpan()
        {
            cacheService.Set("abc", "abcValue", TimeSpan.FromSeconds(5));
            var res = cacheService.Get<string>("abc");
            Assert.AreEqual(true, res.Exists);
            Assert.AreEqual("abcValue", res.Value);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowArgumentNullExceptionIfSetNullValueWithAbsoluteDateTimeOffset()
        {
            cacheService.Set<string>("abc", null, DateTimeOffset.Now.AddSeconds(1));
        }
        [TestMethod]
        public void ShouldReturnCachedValueIfAlreadySetValueWithAbsoluteDateTimeOffset()
        {
            cacheService.Set("abc", "abcValue", DateTimeOffset.Now.AddSeconds(1));
            var res = cacheService.Get<string>("abc");
            Assert.AreEqual(true, res.Exists);
            Assert.AreEqual("abcValue", res.Value);
        }
        [TestMethod]
        public void ShouldReturnDefaultValueIfRemoveKey()
        {
            cacheService.Set("abc", "abcValue", TimeSpan.FromSeconds(5));
            cacheService.RemoveByKey("abc");
            var res = cacheService.Get<string>("abc");
            Assert.AreEqual(false, res.Exists);
            Assert.AreEqual(default, res.Value);
        }

        [TestMethod]
        public async Task ShouldReturnCachedValueIfNotExpiredSlidingTimeSpan()
        {
            cacheService.Set("abc", "abcValue", TimeSpan.FromMilliseconds(1000));
            await Task.Delay(500);
            var res = cacheService.Get<string>("abc");
            Assert.AreEqual(true, res.Exists);
            Assert.AreEqual("abcValue", res.Value);
        }

        [TestMethod]
        public async Task ShouldReturnDefaultValueIfExpiredSlidingTimeSpan()
        {
            cacheService.Set("abc", "abcValue", TimeSpan.FromMilliseconds(1000));
            await Task.Delay(1000);
            var res = cacheService.Get<string>("abc");
            Assert.AreEqual(false, res.Exists);
            Assert.AreEqual(default, res.Value);
        }


        [TestMethod]
        public async Task ShouldReturnCachedValueIfNotExpiredAbsoluteDateTimeOffset()
        {
            cacheService.Set("abc", "abcValue", DateTimeOffset.Now.AddMilliseconds(1000));
            await Task.Delay(500);
            var res = cacheService.Get<string>("abc");
            Assert.AreEqual(true, res.Exists);
            Assert.AreEqual("abcValue", res.Value);
        }

        [TestMethod]
        public async Task ShouldReturnDefaultValueIfExpiredAbsoluteDateTimeOffset()
        {
            cacheService.Set("abc", "abcValue", DateTimeOffset.Now.AddMilliseconds(1000));
            await Task.Delay(1000);
            var res = cacheService.Get<string>("abc");
            Assert.AreEqual(false, res.Exists);
            Assert.AreEqual(default, res.Value);
        }

        [TestMethod]
        public async Task ShouldReturnDefaultValueIfExpiredAbsoluteDateTimeOffsetEvenCalledBefore()
        {
            cacheService.Set("abc", "abcValue", DateTimeOffset.Now.AddMilliseconds(1000));
            await Task.Delay(500);
            var res1 = cacheService.Get<string>("abc");
            Assert.AreEqual(true, res1.Exists);
            Assert.AreEqual("abcValue", res1.Value);
            await Task.Delay(500);
            var res2 = cacheService.Get<string>("abc");
            Assert.AreEqual(false, res2.Exists);
            Assert.AreEqual(default, res2.Value);
        }

        [TestMethod]
        public async Task ShouldReturnCachedValueIfCalledInTimespan()
        {
            cacheService.Set("abc", "abcValue", TimeSpan.FromMilliseconds(1000));
            await Task.Delay(800);
            var res1 = cacheService.Get<string>("abc");
            Assert.AreEqual(true, res1.Exists);
            Assert.AreEqual("abcValue", res1.Value);
            await Task.Delay(800);
            var res2 = cacheService.Get<string>("abc");
            Assert.AreEqual(true, res2.Exists);
            Assert.AreEqual("abcValue", res2.Value);
        }
    }
}
