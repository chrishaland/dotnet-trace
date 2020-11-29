using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using Haland.DotNetTrace;
using Microsoft.AspNetCore.Http;
#if NETCOREAPP3_1
using Microsoft.AspNetCore.Http.Json;
#endif

[SetUpFixture]
public class TestServerSetup
{
    private static IHost _host;
    internal static HttpClient Client;

    [OneTimeSetUp]
    public async Task Before()
    {
        var hostBuilder = new HostBuilder()
            .ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer();
                webHost.ConfigureServices(services =>
                {
                    services.AddTracing();
                });
                webHost.Configure(app =>
                {
                    app.UseTracing();

                    app.Map("/traces", app => app.Run(async context =>
                    {
                        var traces = context.RequestServices.GetRequiredService<TraceMetadata>();
                        await context.Response.WriteAsJsonAsync(traces);
                    }));
                });
            });

        _host = await hostBuilder.StartAsync();
        Client = _host.GetTestClient();
    }

    [OneTimeTearDown]
    public async Task After()
    {
        await _host.StopAsync();
        Client = null;
    }
}


