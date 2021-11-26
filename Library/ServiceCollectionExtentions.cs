namespace Haland.DotNetTrace;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTracing(this IServiceCollection services)
    {
        services.TryAddScoped<TraceMetadata>();
        services.TryAddTransient<HttpMessageHandlerBuilder, TraceHttpMessageHandlerBuilder>();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        return services;
    }
}
