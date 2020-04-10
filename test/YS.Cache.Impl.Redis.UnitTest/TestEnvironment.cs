using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using YS.Knife.Test;

namespace YS.Cache.Impl.Redis
{
    [TestClass]
    public class TestEnvironment
    {
        [AssemblyInitialize()]
        public static void Setup(TestContext assemblyTestContext)
        {
            var availablePort = Utility.GetAvailableTcpPort(6379);
            StartContainer(availablePort);
            SetConnectionString(availablePort);
        }

        [AssemblyCleanup()]
        public static void TearDown()
        {
            DockerCompose.Down();
        }
        private static void StartContainer(uint port)
        {
            DockerCompose.Up(new Dictionary<string, object>
            {
                ["REDIS_PORT"] = port
            });
        }
        private static void SetConnectionString(uint port)
        {

            Environment.SetEnvironmentVariable("RedisCache__ConnectionStrings", $"localhost:{port}");
        }
    }
}
