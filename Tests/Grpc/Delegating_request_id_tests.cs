using System.Threading.Tasks;
using Grpc.Core;
using GrpcTracer;
using NUnit.Framework;

namespace Tests.Grpc
{
    [TestFixture]
    public class Delegating_request_id_tests
    {
        private const string RequestId = "80f198ee56343ba864fe8b2a57d3eff7";

        [Test]
        public async Task Request_id_should_be_created_and_passed_through_when_request_header_does_not_exist()
        {
            var traces = await SUT.GrpcClient2.GetTracesAsync(new TraceRequest());

            Assert.Multiple(() =>
            {
                Assert.That(traces.RequestId, Is.Not.Null);
                Assert.That(traces.RequestId, Is.Not.Empty);
            });
        }

        [Test]
        public async Task Request_id_should_be_passed_thourgh_when_request_header_exist()
        {
            var headers = new Metadata
            {
                { "x-request-id", RequestId }
            };

            var traces = await SUT.GrpcClient2.GetTracesAsync(new TraceRequest(), headers);
            Assert.That(traces.RequestId, Is.EqualTo(RequestId));
        }
    }
}
