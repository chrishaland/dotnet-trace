using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Haland.DotNetTrace;

internal class TraceMiddleware
{
    private readonly RequestDelegate _next;

    public TraceMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var traces = context.RequestServices.GetRequiredService<TraceMetadata>();

        var requestId = context.Request.Headers[Headers.RequestId];
        if (requestId == StringValues.Empty) requestId = Guid.NewGuid().ToString();
        traces.RequestId = requestId;

        var traceId = context.Request.Headers[Headers.TraceId];
        if (traceId != StringValues.Empty) traces.TraceId = traceId;

        var spanId = context.Request.Headers[Headers.SpanId];
        if (spanId != StringValues.Empty) traces.SpanId = spanId;

        var parentSpanId = context.Request.Headers[Headers.ParentSpanId];
        if (parentSpanId != StringValues.Empty) traces.ParentSpanId = parentSpanId;

        var sampled = context.Request.Headers[Headers.Sampled];
        if (sampled != StringValues.Empty) traces.Sampled = sampled;

        var flags = context.Request.Headers[Headers.Flags];
        if (flags != StringValues.Empty) traces.Flags = flags;

        var b3 = context.Request.Headers[Headers.B3];
        if (b3 != StringValues.Empty) traces.B3 = b3;

        var properties = new Dictionary<string, object>
            {
                { "__requestId", requestId }
            };

        var logger = context.RequestServices.GetRequiredService<ILogger<TraceMiddleware>>();
        using (logger.BeginScope(properties))
        {
            await _next(context);
        }
    }
}
