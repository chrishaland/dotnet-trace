using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Tests.Utilities;

namespace Tests.Http
{
    [TestFixture]
    public class Delegating_request_id_tests
    {
        private const string RequestId = "80f198ee56343ba864fe8b2a57d3eff7";

        [TestCase("/basic")]
        [TestCase("/named")]
        [TestCase("/typed")]
        public async Task Request_id_should_be_created_and_passed_through_when_request_header_does_not_exist(string requestUri)
        {
            var traces = await SUT.SendHttpRequest(new HttpRequestMessage(HttpMethod.Get, requestUri), SUT.HttpClient2);

            Assert.Multiple(() =>
            {
                Assert.That(traces.RequestId, Is.Not.Null);
                Assert.That(traces.RequestId, Is.Not.Empty);
            });
        }

        [TestCase("/basic")]
        [TestCase("/named")]
        [TestCase("/typed")]
        public async Task Request_id_should_be_passed_thourgh_when_request_header_exist(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("x-request-id", RequestId);

            var traces = await SUT.SendHttpRequest(request, SUT.HttpClient2);
            AssertRequestIdIsSetProperly(traces);
        }

        [TestCase("/basic")]
        [TestCase("/named")]
        [TestCase("/typed")]
        public async Task Request_id_should_be_passed_thourgh_when_request_content_header_exist(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Content = new StringContent("");
            request.Content.Headers.Add("x-request-id", RequestId);

            var traces = await SUT.SendHttpRequest(request, SUT.HttpClient2);
            AssertRequestIdIsSetProperly(traces);
        }

        private static void AssertRequestIdIsSetProperly(TraceMetadataResponse traces)
        {
            Assert.That(traces.RequestId, Is.EqualTo(RequestId));
        }
    }
}
