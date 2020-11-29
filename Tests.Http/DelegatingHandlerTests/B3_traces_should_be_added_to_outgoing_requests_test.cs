using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Tests.HttpTestServerUtilities;

namespace Tests.Http
{
    [TestFixture]
    public class B3_traces_should_be_added_to_outgoing_requests_test
    {
        private const string TraceId = "80f198ee56343ba864fe8b2a57d3eff7";
        private const string SpanId = "e457b5a2e4d86bd1";
        private const string ParentSpanId = "05e3ac9a4f6e3b90";
        private const string Sampled = "1";
        private const string Flags = "1";
        private static readonly string _b3 = $"{TraceId}-{SpanId}-{Sampled}-{ParentSpanId}";

        //[TestCase("/basic")]
        [TestCase("/named")]
        [TestCase("/typed")]
        public async Task B3_traces_should_be_added_to_outgoing_requests(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("x-b3-traceid", TraceId);
            request.Headers.Add("x-b3-spanid", SpanId);
            request.Headers.Add("x-b3-parentspanid", ParentSpanId);
            request.Headers.Add("x-b3-sampled", Sampled);
            request.Headers.Add("x-b3-flags", Flags);
            request.Headers.Add("b3", _b3);

            var traces = await SUT.SendRequest(request, SUT.TestServer2);
            AssertTracesAreSetProperly(traces);
        }

        //[TestCase("/basic")]
        [TestCase("/named")]
        [TestCase("/typed")]
        public async Task B3_traces_should_be_added_to_outgoing_requests_from_content_headers(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Content = new StringContent("");
            request.Content.Headers.Add("x-b3-traceid", TraceId);
            request.Content.Headers.Add("x-b3-spanid", SpanId);
            request.Content.Headers.Add("x-b3-parentspanid", ParentSpanId);
            request.Content.Headers.Add("x-b3-sampled", Sampled);
            request.Content.Headers.Add("x-b3-flags", Flags);
            request.Content.Headers.Add("b3", _b3);

            var traces = await SUT.SendRequest(request, SUT.TestServer2);
            AssertTracesAreSetProperly(traces);
        }

        private static void AssertTracesAreSetProperly(TraceMetadataResponse traces)
        {
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
}
