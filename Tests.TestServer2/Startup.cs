using GrpcTracer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.TestServer2;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTracing();
        services.AddRouting();
        services.AddControllers();
        services.AddGrpc();
        services.AddGrpcClient<Tracer.TracerClient>(options =>
        {
            options.Address = new Uri("https://localhost:5000");
        });
        services.AddHttpClient();
        services.AddHttpClient("named", client =>
        {
            client.BaseAddress = new Uri("https://localhost:5000");
        });
        services.AddHttpClient<TypedHttpClient>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseTracing();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGrpcService<GrpcService>();
        });
    }
}
