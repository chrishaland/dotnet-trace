using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Tests.Utilities;

namespace Tests.Http
{
    [TestFixture]
    public class Middleware_request_id_tests
    {
        private const string RequestId = "80f198ee56343ba864fe8b2a57d3eff7";

        [Test]
        public async Task Request_id_should_be_created_when_request_header_does_not_exist()
        {
            var traces = await SUT.SendHttpRequest(new HttpRequestMessage(HttpMethod.Get, "/"), SUT.HttpClient1);

            Assert.Multiple(() =>
            {
                Assert.That(traces.RequestId, Is.Not.Null);
                Assert.That(traces.RequestId, Is.Not.Empty);
            });
        }

        [Test]
        public async Task Request_id_should_be_set_when_request_header_exists()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/");
            request.Headers.Add("x-request-id", RequestId);

            var traces = await SUT.SendHttpRequest(request, SUT.HttpClient1);
            AssertRequestIdIsSetProperly(traces);
        }

        [Test]
        public async Task Request_id_should_be_set_when_request_content_header_exists()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/");
            request.Content = new StringContent("");
            request.Content.Headers.Add("x-request-id", RequestId);

            var traces = await SUT.SendHttpRequest(request, SUT.HttpClient1);
            AssertRequestIdIsSetProperly(traces);
        }

        private static void AssertRequestIdIsSetProperly(TraceMetadataResponse traces)
        {
            Assert.That(traces.RequestId, Is.EqualTo(RequestId));
        }
    }
}
