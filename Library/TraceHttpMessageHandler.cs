using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Haland.DotNetTrace
{
    public class TraceHttpMessageHandler : DelegatingHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public TraceHttpMessageHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var traces = _serviceProvider.GetRequiredService<TraceMetadata>();

            request.Headers.Add(Headers.RequestId, traces.RequestId);

            if (!string.IsNullOrEmpty(traces.TraceId))
            {
                request.Headers.Add(Headers.TraceId, traces.TraceId);
            }

            if (!string.IsNullOrEmpty(traces.SpanId))
            {
                request.Headers.Add(Headers.SpanId, traces.SpanId);
            }

            if (!string.IsNullOrEmpty(traces.ParentSpanId))
            {
                request.Headers.Add(Headers.ParentSpanId, traces.ParentSpanId);
            }

            if (!string.IsNullOrEmpty(traces.Sampled))
            {
                request.Headers.Add(Headers.Sampled, traces.Sampled);
            }

            if (!string.IsNullOrEmpty(traces.Flags))
            {
                request.Headers.Add(Headers.Flags, traces.Flags);
            }

            if (!string.IsNullOrEmpty(traces.B3))
            {
                request.Headers.Add(Headers.B3, traces.B3);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
