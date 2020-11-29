using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests.Http
{
    [TestFixture]
    public class Request_id_should_be_created_and_passed_through_when_request_header_does_not_exist_test
    {
        //[TestCase("/basic")]
        [TestCase("/named")]
        [TestCase("/typed")]
        public async Task Request_id_should_be_created_and_passed_through_when_request_header_does_not_exist(string requestUri)
        {
            var traces = await SUT.SendRequest(new HttpRequestMessage(HttpMethod.Get, requestUri), SUT.TestServer2);

            Assert.Multiple(() =>
             {
                Assert.That(traces.RequestId, Is.Not.Null);
                Assert.That(traces.RequestId, Is.Not.Empty);
            });
        }
    }
}
