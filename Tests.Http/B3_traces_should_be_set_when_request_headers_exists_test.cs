using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests.Http
{
    [TestFixture]
    public class B3_traces_should_be_set_when_request_headers_exists_test
    {
        [Test]
        public async Task B3_traces_should_be_set_when_request_headers_exists()
        {
            var traceId = "80f198ee56343ba864fe8b2a57d3eff7";
            var spanId = "e457b5a2e4d86bd1";
            var parentSpanId = "05e3ac9a4f6e3b90";
            var sampled = "1";
            var flags = "1";
            var b3 = $"{traceId}-{spanId}-{sampled}-{parentSpanId}";

            var request = new HttpRequestMessage(HttpMethod.Get, "/");
            request.Headers.Add("x-b3-traceid", traceId);
            request.Headers.Add("x-b3-spanid", spanId);
            request.Headers.Add("x-b3-parentspanid", parentSpanId);
            request.Headers.Add("x-b3-sampled", sampled);
            request.Headers.Add("x-b3-flags", flags);
            request.Headers.Add("b3", b3);

            var response = await TestHost.SUT.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            using var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var traces = await JsonSerializer.DeserializeAsync<TraceMetadataResponse>(contentStream, TestHost.SerializerOptions);

            Assert.Multiple(() =>
            {
                Assert.That(traces.TraceId, Is.EqualTo(traceId));
                Assert.That(traces.SpanId, Is.EqualTo(spanId));
                Assert.That(traces.ParentSpanId, Is.EqualTo(parentSpanId));
                Assert.That(traces.Sampled, Is.EqualTo(sampled));
                Assert.That(traces.Flags, Is.EqualTo(flags));
                Assert.That(traces.B3, Is.EqualTo(b3));
            });
        }

        [Test]
        public async Task B3_traces_should_be_set_when_request_content_headers_exists()
        {
            var traceId = "80f198ee56343ba864fe8b2a57d3eff7";
            var spanId = "e457b5a2e4d86bd1";
            var parentSpanId = "05e3ac9a4f6e3b90";
            var sampled = "1";
            var flags = "1";
            var b3 = $"{traceId}-{spanId}-{sampled}-{parentSpanId}";

            var request = new HttpRequestMessage(HttpMethod.Post, "/")
            {
                Content = new StringContent("")
            };
            request.Content.Headers.Add("x-b3-traceid", traceId);
            request.Content.Headers.Add("x-b3-spanid", spanId);
            request.Content.Headers.Add("x-b3-parentspanid", parentSpanId);
            request.Content.Headers.Add("x-b3-sampled", sampled);
            request.Content.Headers.Add("x-b3-flags", flags);
            request.Content.Headers.Add("b3", b3);

            var response = await TestHost.SUT.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            using var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var traces = await JsonSerializer.DeserializeAsync<TraceMetadataResponse>(contentStream, TestHost.SerializerOptions);

            Assert.Multiple(() =>
            {
                Assert.That(traces.TraceId, Is.EqualTo(traceId));
                Assert.That(traces.SpanId, Is.EqualTo(spanId));
                Assert.That(traces.ParentSpanId, Is.EqualTo(parentSpanId));
                Assert.That(traces.Sampled, Is.EqualTo(sampled));
                Assert.That(traces.Flags, Is.EqualTo(flags));
                Assert.That(traces.B3, Is.EqualTo(b3));
            });
        }
    }
}
