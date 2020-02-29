using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace YS.Cache
{
    public abstract class CacheServiceTestBase : Knife.Hosting.KnifeHost
    {
        public CacheServiceTestBase()
        {
            this.TestObject = this.Get<ICacheService>();
        }
        private ICacheService TestObject;
        [TestMethod]
        public async Task ShouldReturnDefaultValueIfNotContainsKey()
        {
            var res = await TestObject.Get<string>("abc");
            Assert.AreEqual(false, res.Exists);
            Assert.AreEqual(default, res.Value);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ShouldThrowArgumentNullExceptionIfSetNullValueWithSlidingTimeSpan()
        {
            await TestObject.Set<string>("abc", null, TimeSpan.FromSeconds(5));
        }
        [TestMethod]
        public async Task ShouldReturnCachedValueIfAlreadySetValueWithSlidingTimeSpan()
        {
            await TestObject.Set("abc", "abcValue", TimeSpan.FromSeconds(5));
            var res = await TestObject.Get<string>("abc");
            Assert.AreEqual(true, res.Exists);
            Assert.AreEqual("abcValue", res.Value);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ShouldThrowArgumentNullExceptionIfSetNullValueWithAbsoluteDateTimeOffset()
        {
            await TestObject.Set<string>("abc", null, DateTimeOffset.Now.AddSeconds(5));
        }
        [TestMethod]
        public async Task ShouldReturnCachedValueIfAlreadySetValueWithAbsoluteDateTimeOffset()
        {
            await TestObject.Set("abc", "abcValue", DateTimeOffset.Now.AddSeconds(5));
            var res = await TestObject.Get<string>("abc");
            Assert.AreEqual(true, res.Exists);
            Assert.AreEqual("abcValue", res.Value);
        }
        [TestMethod]
        public async Task ShouldReturnDefaultValueIfRemoveKey()
        {
            await TestObject.Set("abc", "abcValue", TimeSpan.FromSeconds(5));
            await TestObject.RemoveByKey("abc");
            var res = await TestObject.Get<string>("abc");
            Assert.AreEqual(false, res.Exists);
            Assert.AreEqual(default, res.Value);
        }

        [TestMethod]
        public async Task ShouldReturnCachedValueIfNotExpiredSlidingTimeSpan()
        {
            await TestObject.Set("abc", "abcValue", TimeSpan.FromMilliseconds(1000));
            await Task.Delay(500);
            var res = await TestObject.Get<string>("abc");
            Assert.AreEqual(true, res.Exists);
            Assert.AreEqual("abcValue", res.Value);
        }

        [TestMethod]
        public async Task ShouldReturnDefaultValueIfExpiredSlidingTimeSpan()
        {
            await TestObject.Set("abc", "abcValue", TimeSpan.FromMilliseconds(1000));
            await Task.Delay(1100);
            var res = await TestObject.Get<string>("abc");
            Assert.AreEqual(false, res.Exists);
            Assert.AreEqual(default, res.Value);
        }


        [TestMethod]
        public async Task ShouldReturnCachedValueIfNotExpiredAbsoluteDateTimeOffset()
        {
            await TestObject.Set("abc", "abcValue", DateTimeOffset.Now.AddMilliseconds(5000));
            await Task.Delay(500);
            var res = await TestObject.Get<string>("abc");
            Assert.AreEqual(true, res.Exists);
            Assert.AreEqual("abcValue", res.Value);
        }

        [TestMethod]
        public async Task ShouldReturnDefaultValueIfExpiredAbsoluteDateTimeOffset()
        {
            await TestObject.Set("abc", "abcValue", DateTimeOffset.Now.AddMilliseconds(1000));
            await Task.Delay(1100);
            var res = await TestObject.Get<string>("abc");
            Assert.AreEqual(false, res.Exists);
            Assert.AreEqual(default, res.Value);
        }

        [TestMethod]
        public async Task ShouldReturnDefaultValueIfExpiredAbsoluteDateTimeOffsetEvenCalledBefore()
        {
            await TestObject.Set("abc", "abcValue", DateTimeOffset.Now.AddMilliseconds(5000));
            await Task.Delay(3000);
            var res1 = await TestObject.Get<string>("abc");
            Assert.AreEqual(true, res1.Exists);
            Assert.AreEqual("abcValue", res1.Value);
            await Task.Delay(3000);
            var res2 = await TestObject.Get<string>("abc");
            Assert.AreEqual(false, res2.Exists);
            Assert.AreEqual(default, res2.Value);
        }

        [TestMethod]
        public async Task ShouldReturnCachedValueIfCalledInTimespan()
        {
            await TestObject.Set("abc", "abcValue", TimeSpan.FromMilliseconds(1000));
            await Task.Delay(800);
            var res1 = await TestObject.Get<string>("abc");
            Assert.AreEqual(true, res1.Exists);
            Assert.AreEqual("abcValue", res1.Value);
            await Task.Delay(800);
            var res2 = await TestObject.Get<string>("abc");
            Assert.AreEqual(true, res2.Exists);
            Assert.AreEqual("abcValue", res2.Value);
        }
        [TestMethod]
        public async Task ShouldSuccessWhenSetAndGetObject()
        {
            string key = RandomUtility.RandomVarName(16);
            var user = new User { Name = "ZhangSan", Age = 16, Birthday = DateTimeOffset.Now };
            await TestObject.Set(key, user, TimeSpan.FromSeconds(5));
            var cachedUser = await TestObject.Get<User>(key);
            Assert.IsTrue(cachedUser.Exists);
            Assert.AreEqual(user.Name, cachedUser.Value.Name);
            Assert.AreEqual(user.Age, cachedUser.Value.Age);
            Assert.AreEqual(user.Birthday, cachedUser.Value.Birthday);
        }
        private class User
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public DateTimeOffset Birthday { get; set; }
        }
    }
}
