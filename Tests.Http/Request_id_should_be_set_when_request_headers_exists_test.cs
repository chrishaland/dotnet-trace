using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests.Http
{
    [TestFixture]
    public class Request_id_should_be_set_when_request_headers_exists_test
    {
        [Test]
        public async Task Request_id_should_be_set_when_request_header_exists()
        {
            var requestId = "80f198ee56343ba864fe8b2a57d3eff7";

            var request = new HttpRequestMessage(HttpMethod.Get, "/");
            request.Headers.Add("x-request-id", requestId);

            var response = await TestHost.SUT.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            using var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var traces = await JsonSerializer.DeserializeAsync<TraceMetadataResponse>(contentStream, TestHost.SerializerOptions);

            Assert.That(traces.RequestId, Is.EqualTo(requestId));
        }

        [Test]
        public async Task Request_id_should_be_set_when_request_content_header_exists()
        {
            var requestId = "80f198ee56343ba864fe8b2a57d3eff7";

            var request = new HttpRequestMessage(HttpMethod.Post, "/")
            {
                Content = new StringContent("")
            };
            request.Content.Headers.Add("x-request-id", requestId);

            var response = await TestHost.SUT.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            using var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var traces = await JsonSerializer.DeserializeAsync<TraceMetadataResponse>(contentStream, TestHost.SerializerOptions);

            Assert.That(traces.RequestId, Is.EqualTo(requestId));
        }
    }
}
