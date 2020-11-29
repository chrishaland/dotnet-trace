using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

[SetUpFixture]
public class TestServerSetup
{
    private static TestServer _testServer1;
    private static TestServer _testServer2;

    internal static HttpClient TestServer1;
    internal static HttpClient TestServer2;

    [OneTimeSetUp]
    public async Task Before()
    {
        _testServer1 = new TestServer(TestServer1Builder);
        await _testServer1.Host.StartAsync();
        TestServer1 = _testServer1.CreateClient();

        _testServer2 = new TestServer(TestServer2Builder(_testServer1.CreateHandler()));
        await _testServer2.Host.StartAsync();
        TestServer2 = _testServer2.CreateClient();
    }

    [OneTimeTearDown]
    public async Task After()
    {
        await _testServer1?.Host?.StopAsync();
        _testServer1?.Dispose();
        TestServer1 = null;

        await _testServer2?.Host?.StopAsync();
        _testServer2?.Dispose();
        TestServer2 = null;
    }

    private static IWebHostBuilder TestServer1Builder => new WebHostBuilder()
        .UseConfiguration(new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).Build())
        .UseStartup<Tests.HttpTestServer1.Startup>();

    private static IWebHostBuilder TestServer2Builder(HttpMessageHandler handler) => new WebHostBuilder()
        .UseConfiguration(new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).Build())
        .UseStartup<Tests.HttpTestServer2.Startup>()
        .ConfigureTestServices(services =>
        {
            //services.AddHttpClient().ConfigurePrimaryHttpMessageHandler(_ => handler);
            services.AddHttpClient("named").ConfigurePrimaryHttpMessageHandler(_ => handler);
            services.AddHttpClient<Tests.HttpTestServer2.TypedHttpClient>().ConfigurePrimaryHttpMessageHandler(_ => handler);
        });
}


