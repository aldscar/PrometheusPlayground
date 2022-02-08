using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NUnit.Framework;
using Serilog;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PromethesPlaygroundTests
{
    public class MeasuredHostedServiceMethods
    {
        private HttpClient? client;
        private TestServer? server;

        [SetUp]
        public void Setup()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseConfiguration(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build())
                .UseSerilog((ctx, config) => config.ReadFrom.Configuration(ctx.Configuration))
                .UseStartup<MeasuredHostedService.Startup>()
                .UseTestServer();

            server = new TestServer(builder);
            client = server.CreateClient();
        }

        [TearDown]
        public void End()
        {            
            server?.Dispose();
        }

        [Test]
        public async Task CallHealthCheckEndpoint()
        {
            var output = await Send(HttpMethod.Get, "/health");
            Assert.IsNotNull(output);

            var statuses = new String[] { HealthStatus.Healthy.ToString(), HealthStatus.Unhealthy.ToString(), HealthStatus.Degraded.ToString() };
            Assert.IsTrue(statuses.Contains(output));
        }
        [Test]
        public async Task CallMetricsEndpoint()
        {
            var output = await Send(HttpMethod.Get, "/metrics");
            Assert.IsNotNull(output);
            Assert.IsTrue(output.StartsWith("#"));
        }

        [Test]
        public async Task CallTerminalEndpoint()
        {
            var output = await Send(HttpMethod.Get, "/");
            Assert.IsNotNull(output);
            Assert.IsTrue(output == "Go to /health to see the health status");
        }

        protected async Task<String> Send(HttpMethod method, String url, String? content = null)
        {
            var message = new HttpRequestMessage(method, url);

            if (content != null)
            {
                message.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }

            var result = await client.SendAsync(message);

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException(result.ReasonPhrase);
            }

            return result.Content == null ? String.Empty : await result.Content.ReadAsStringAsync();
        }
    }
}