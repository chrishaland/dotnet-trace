using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests.Http
{
    [TestFixture]
    public class Request_id_should_be_created_when_request_header_does_not_exist_test
    {
        [Test]
        public async Task Request_id_should_be_created_when_request_header_does_not_exist()
        {
            var response = await TestHost.SUT.GetAsync("/");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            using var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var traces = await JsonSerializer.DeserializeAsync<TraceMetadataResponse>(contentStream, TestHost.SerializerOptions);

            Assert.Multiple(() =>
             {
                Assert.That(traces.RequestId, Is.Not.Null);
                Assert.That(traces.RequestId, Is.Not.Empty);
            });
        }
    }
}
