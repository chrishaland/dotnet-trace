using Grpc.Core;
using GrpcTracer;

namespace Tests.Grpc;

[TestFixture]
public class Delegating_B3_traces_tests
{
    private const string TraceId = "80f198ee56343ba864fe8b2a57d3eff7";
    private const string SpanId = "e457b5a2e4d86bd1";
    private const string ParentSpanId = "05e3ac9a4f6e3b90";
    private const string Sampled = "1";
    private const string Flags = "1";
    private static readonly string _b3 = $"{TraceId}-{SpanId}-{Sampled}-{ParentSpanId}";

    [Test]
    public async Task B3_traces_should_not_be_added_to_outgoing_requests_when_request_headers_does_not_exist()
    {
        var traces = await SUT.GrpcClient2.GetTracesAsync(new TraceRequest());

        Assert.Multiple(() =>
        {
            Assert.That(traces.TraceId, Is.Empty);
            Assert.That(traces.SpanId, Is.Empty);
            Assert.That(traces.ParentSpanId, Is.Empty);
            Assert.That(traces.ParentSpanId, Is.Empty);
            Assert.That(traces.Sampled, Is.Empty);
            Assert.That(traces.Flags, Is.Empty);
            Assert.That(traces.B3, Is.Empty);
        });
    }

    [Test]
    public async Task B3_traces_should_be_added_to_outgoing_requests()
    {
        var headers = new Metadata
        {
            { "x-b3-traceid", TraceId },
            { "x-b3-spanid", SpanId },
            { "x-b3-parentspanid", ParentSpanId },
            { "x-b3-sampled", Sampled },
            { "x-b3-flags", Flags },
            { "b3", _b3 }
        };

        var traces = await SUT.GrpcClient2.GetTracesAsync(new TraceRequest(), headers);
        Assert.Multiple(() =>
        {
            Assert.That(traces.TraceId, Is.EqualTo(TraceId));
            Assert.That(traces.SpanId, Is.EqualTo(SpanId));
            Assert.That(traces.ParentSpanId, Is.EqualTo(ParentSpanId));
            Assert.That(traces.Sampled, Is.EqualTo(Sampled));
            Assert.That(traces.Flags, Is.EqualTo(Flags));
            Assert.That(traces.B3, Is.EqualTo(_b3));
        });
    }
}
