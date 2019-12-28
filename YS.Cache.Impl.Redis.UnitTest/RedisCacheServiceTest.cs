using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YS.Docker;

namespace YS.Cache.Impl.Redis.UnitTest
{
    [TestClass]
    public class RedisCacheServiceTest : CacheServiceTestBase
    {
        static DockerContainerSettings RedisDockerSetting = new DockerContainerSettings
        {
            ImageName = "redis",
            Ports = new Dictionary<int, int>
            {
                [6379] = 6379
            }
        };
        protected override void OnSetup()
        {
            base.OnSetup();
            this.dockerContainerService = this.Get<IDockerContainerService>();
            this.containerId= this.dockerContainerService.RunAsync(RedisDockerSetting).Result;
           
        }
        private string containerId=null;
        private IDockerContainerService dockerContainerService;

        protected override void OnTearDown()
        {
            if (!string.IsNullOrEmpty(containerId))
            {
                this.dockerContainerService.StopAsync(containerId).Wait();
            }
            base.OnTearDown();
           
        }
     
    }
}
