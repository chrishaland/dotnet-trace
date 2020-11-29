using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Tests.HttpTestServerUtilities;

namespace Tests.Http
{
    [TestFixture]
    public class Request_id_should_be_set_when_request_headers_exists_test
    {
        private const string RequestId = "80f198ee56343ba864fe8b2a57d3eff7";

        [Test]
        public async Task Request_id_should_be_set_when_request_header_exists()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/");
            request.Headers.Add("x-request-id", RequestId);

            var traces = await SUT.SendRequest(request, SUT.TestServer1);
            AssertRequestIdIsSetProperly(traces);
        }

        [Test]
        public async Task Request_id_should_be_set_when_request_content_header_exists()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/");
            request.Content = new StringContent("");
            request.Content.Headers.Add("x-request-id", RequestId); 
            
            var traces = await SUT.SendRequest(request, SUT.TestServer1);
            AssertRequestIdIsSetProperly(traces);
        }

        private static void AssertRequestIdIsSetProperly(TraceMetadataResponse traces)
        {
            Assert.That(traces.RequestId, Is.EqualTo(RequestId));
        }
    }
}
