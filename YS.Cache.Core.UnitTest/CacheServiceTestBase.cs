using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace YS.Cache
{
    public abstract class CacheServiceTestBase:Knife.Hosting.KnifeHost
    {
        public CacheServiceTestBase()
        {
            this.TestObject = this.Get<ICacheService>();
        }
        private ICacheService TestObject;
        [TestMethod]
        public void ShouldReturnDefaultValueIfNotContainsKey()
        {
            var res = TestObject.Get<string>("abc");
            Assert.AreEqual(false, res.Exists);
            Assert.AreEqual(default, res.Value);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowArgumentNullExceptionIfSetNullValueWithSlidingTimeSpan()
        {
            TestObject.Set<string>("abc", null, TimeSpan.FromSeconds(5));
        }
        [TestMethod]
        public void ShouldReturnCachedValueIfAlreadySetValueWithSlidingTimeSpan()
        {
            TestObject.Set("abc", "abcValue", TimeSpan.FromSeconds(5));
            var res = TestObject.Get<string>("abc");
            Assert.AreEqual(true, res.Exists);
            Assert.AreEqual("abcValue", res.Value);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowArgumentNullExceptionIfSetNullValueWithAbsoluteDateTimeOffset()
        {
            TestObject.Set<string>("abc", null, DateTimeOffset.Now.AddSeconds(5));
        }
        [TestMethod]
        public void ShouldReturnCachedValueIfAlreadySetValueWithAbsoluteDateTimeOffset()
        {
            TestObject.Set("abc", "abcValue", DateTimeOffset.Now.AddSeconds(5));
            var res = TestObject.Get<string>("abc");
            Assert.AreEqual(true, res.Exists);
            Assert.AreEqual("abcValue", res.Value);
        }
        [TestMethod]
        public void ShouldReturnDefaultValueIfRemoveKey()
        {
            TestObject.Set("abc", "abcValue", TimeSpan.FromSeconds(5));
            TestObject.RemoveByKey("abc");
            var res = TestObject.Get<string>("abc");
            Assert.AreEqual(false, res.Exists);
            Assert.AreEqual(default, res.Value);
        }

        [TestMethod]
        public async Task ShouldReturnCachedValueIfNotExpiredSlidingTimeSpan()
        {
            TestObject.Set("abc", "abcValue", TimeSpan.FromMilliseconds(1000));
            await Task.Delay(500);
            var res = TestObject.Get<string>("abc");
            Assert.AreEqual(true, res.Exists);
            Assert.AreEqual("abcValue", res.Value);
        }

        [TestMethod]
        public async Task ShouldReturnDefaultValueIfExpiredSlidingTimeSpan()
        {
            TestObject.Set("abc", "abcValue", TimeSpan.FromMilliseconds(1000));
            await Task.Delay(1100);
            var res = TestObject.Get<string>("abc");
            Assert.AreEqual(false, res.Exists);
            Assert.AreEqual(default, res.Value);
        }


        [TestMethod]
        public async Task ShouldReturnCachedValueIfNotExpiredAbsoluteDateTimeOffset()
        {
            TestObject.Set("abc", "abcValue", DateTimeOffset.Now.AddMilliseconds(5000));
            await Task.Delay(500);
            var res = TestObject.Get<string>("abc");
            Assert.AreEqual(true, res.Exists);
            Assert.AreEqual("abcValue", res.Value);
        }

        [TestMethod]
        public async Task ShouldReturnDefaultValueIfExpiredAbsoluteDateTimeOffset()
        {
            TestObject.Set("abc", "abcValue", DateTimeOffset.Now.AddMilliseconds(1000));
            await Task.Delay(1100);
            var res = TestObject.Get<string>("abc");
            Assert.AreEqual(false, res.Exists);
            Assert.AreEqual(default, res.Value);
        }

        [TestMethod]
        public async Task ShouldReturnDefaultValueIfExpiredAbsoluteDateTimeOffsetEvenCalledBefore()
        {
            TestObject.Set("abc", "abcValue", DateTimeOffset.Now.AddMilliseconds(5000));
            await Task.Delay(3000);
            var res1 = TestObject.Get<string>("abc");
            Assert.AreEqual(true, res1.Exists);
            Assert.AreEqual("abcValue", res1.Value);
            await Task.Delay(3000);
            var res2 = TestObject.Get<string>("abc");
            Assert.AreEqual(false, res2.Exists);
            Assert.AreEqual(default, res2.Value);
        }

        [TestMethod]
        public async Task ShouldReturnCachedValueIfCalledInTimespan()
        {
            TestObject.Set("abc", "abcValue", TimeSpan.FromMilliseconds(1000));
            await Task.Delay(800);
            var res1 = TestObject.Get<string>("abc");
            Assert.AreEqual(true, res1.Exists);
            Assert.AreEqual("abcValue", res1.Value);
            await Task.Delay(800);
            var res2 = TestObject.Get<string>("abc");
            Assert.AreEqual(true, res2.Exists);
            Assert.AreEqual("abcValue", res2.Value);
        }
    }
}
