using Microsoft.AspNetCore.Builder;

namespace Haland.DotNetTrace;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseTracing(this IApplicationBuilder app)
    {
        return app.UseMiddleware<TraceMiddleware>();
    }
}
