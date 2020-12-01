using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
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
    private static GrpcChannel _grpcChannel1;
    private static GrpcChannel _grpcChannel2;

    internal static HttpClient TestServer1HttpClient;
    internal static GrpcTracer.Tracer.TracerClient TestServer1GrpcClient;
    internal static HttpClient TestServer2HttpClient;
    internal static GrpcTracer.Tracer.TracerClient TestServer2GrpcClient;

    [OneTimeSetUp]
    public async Task Before()
    {
        _testServer1 = new TestServer(TestServer1Builder);
        await _testServer1.Host.StartAsync();
        TestServer1HttpClient = _testServer1.CreateClient();

        _grpcChannel1 = GrpcChannel.ForAddress("https://localhost:5000", 
            new GrpcChannelOptions { HttpHandler = Hander(_testServer1) });
        TestServer1GrpcClient = new GrpcTracer.Tracer.TracerClient(_grpcChannel1);
        
        _testServer2 = new TestServer(TestServer2Builder(Hander(_testServer1)));
        await _testServer2.Host.StartAsync();
        TestServer2HttpClient = _testServer2.CreateClient();

        _grpcChannel2 = GrpcChannel.ForAddress("https://localhost:5010", 
            new GrpcChannelOptions { HttpHandler = Hander(_testServer2) });
        TestServer2GrpcClient = new GrpcTracer.Tracer.TracerClient(_grpcChannel2);
    }

    [OneTimeTearDown]
    public async Task After()
    {
        await _testServer1?.Host?.StopAsync();
        _testServer1?.Dispose();
        _grpcChannel1?.Dispose();
        TestServer1HttpClient = null;
        TestServer1GrpcClient = null;

        await _testServer2?.Host?.StopAsync();
        _testServer2?.Dispose();
        _grpcChannel2?.Dispose();
        TestServer2HttpClient = null;
        TestServer2GrpcClient = null;
    }

    private static HttpMessageHandler Hander(TestServer testServer) =>
#if NETCOREAPP3_1
        new ResponseVersionHandler { InnerHandler = testServer.CreateHandler() };
#else
        testServer.CreateHandler();
#endif

    private static IWebHostBuilder TestServer1Builder => new WebHostBuilder()
        .UseTestServer()
        .UseConfiguration(new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).Build())
        .UseStartup<Tests.TestServer1.Startup>();

    private static IWebHostBuilder TestServer2Builder(HttpMessageHandler handler) => new WebHostBuilder()
        .UseTestServer()
        .UseConfiguration(new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).Build())
        .UseStartup<Tests.TestServer2.Startup>()
        .ConfigureTestServices(services =>
        {
            services.AddGrpcClient<GrpcTracer.Tracer.TracerBase>(options => options.ChannelOptionsActions.Add(_ => _.HttpHandler = handler));
            services.AddHttpClient("").ConfigurePrimaryHttpMessageHandler(_ => handler);
            services.AddHttpClient("named").ConfigurePrimaryHttpMessageHandler(_ => handler);
            services.AddHttpClient<Tests.TestServer2.TypedHttpClient>().ConfigurePrimaryHttpMessageHandler(_ => handler);
        });

    private class ResponseVersionHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            response.Version = request.Version;

            return response;
        }
    }
}


