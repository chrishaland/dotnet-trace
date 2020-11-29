using System;
using Haland.DotNetTrace;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.HttpTestServer2
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTracing();
            services.AddRouting();
            services.AddControllers();
            services.AddHttpClient();
            services.AddHttpClient("named", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000");
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
            });
        }
    }
}
