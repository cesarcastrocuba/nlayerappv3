using DistributedServices.BlogBoundedContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using Xunit;

namespace NLayerApp.DistributedServices.BlogBoundedContext.Tests
{
    public class IntegrationTestsInitialize : IDisposable
    {
        public TestServer server { get; private set; }

        public IntegrationTestsInitialize()
        {
            var hostBuilder = new WebHostBuilder()
                .UseStartup<Startup>();

            server = new TestServer(hostBuilder);
        }
        public void Dispose()
        {

        }
    }

    [CollectionDefinition("Our Test Collection #5")]
    public class Collection5 : ICollectionFixture<IntegrationTestsInitialize>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
