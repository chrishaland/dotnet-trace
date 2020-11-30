using System.Threading.Tasks;
using Grpc.Core;
using GrpcTracer;
using Haland.DotNetTrace;

namespace Tests.TestServer1
{
    public class GrpcService : Tracer.TracerBase
    {
        private readonly TraceMetadata _traces;

        public GrpcService(TraceMetadata traces)
        {
            _traces = traces;
        }

        public override Task<TraceResponse> GetTraces(TraceRequest request, ServerCallContext context)
        {
            return Task.FromResult(new TraceResponse
            {
                RequestId = _traces.RequestId ?? string.Empty,
                SpanId = _traces.SpanId ?? string.Empty,
                ParentSpanId = _traces.ParentSpanId ?? string.Empty,
                TraceId = _traces.TraceId ?? string.Empty,
                Sampled = _traces.Sampled ?? string.Empty,
                Flags = _traces.Flags ?? string.Empty,
                B3 = _traces.B3 ?? string.Empty
            });
        }
    }
}
