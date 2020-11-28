using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests.Http
{
    [TestFixture]
    public class B3_traces_should_not_be_set_when_request_headers_does_not_exist_test
    {
        [Test]
        public async Task B3_traces_should_not_be_set_when_request_headers_does_not_exist()
        {
            var response = await TestHost.SUT.GetAsync("/");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            using var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var traces = await JsonSerializer.DeserializeAsync<TraceMetadataResponse>(contentStream, TestHost.SerializerOptions);

            Assert.Multiple(() =>
            {
                Assert.That(traces.TraceId, Is.Null);
                Assert.That(traces.SpanId, Is.Null);
                Assert.That(traces.ParentSpanId, Is.Null);
                Assert.That(traces.ParentSpanId, Is.Null);
                Assert.That(traces.Sampled, Is.Null);
                Assert.That(traces.Flags, Is.Null);
                Assert.That(traces.B3, Is.Null);
            });
        }
    }
}
