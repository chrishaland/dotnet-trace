using Haland.DotNetTrace;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.HttpTestServer1
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTracing();
            services.AddRouting();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseTracing();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
            });
        }
    }
}
