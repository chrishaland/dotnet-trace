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
            var traces = await SUT.GetTraces();

            Assert.Multiple(() =>
             {
                Assert.That(traces.RequestId, Is.Not.Null);
                Assert.That(traces.RequestId, Is.Not.Empty);
            });
        }
    }
}
