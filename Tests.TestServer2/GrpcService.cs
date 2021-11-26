using Grpc.Core;
using GrpcTracer;

namespace Tests.TestServer2;

public class GrpcService : Tracer.TracerBase
{
    private readonly Tracer.TracerClient _client;

    public GrpcService(Tracer.TracerClient client)
    {
        _client = client;
    }

    public override async Task<TraceResponse> GetTraces(TraceRequest request, ServerCallContext context)
    {
        var traces = await _client.GetTracesAsync(new TraceRequest());
        return traces;
    }
}
