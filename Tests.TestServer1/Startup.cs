using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.TestServer1;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTracing();
        services.AddRouting();
        services.AddControllers();
        services.AddGrpc();
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
