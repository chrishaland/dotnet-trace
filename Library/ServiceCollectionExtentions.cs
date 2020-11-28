using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;

namespace Haland.DotNetTrace
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTracing(this IServiceCollection services)
        {
            services.TryAddScoped<TraceMetadata>();
            services.TryAddTransient<HttpMessageHandlerBuilder, TraceHttpMessageHandlerBuilder>();
            return services;
        }
    }
}
