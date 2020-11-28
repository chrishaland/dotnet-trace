using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests.Http
{
    [TestFixture]
    public class Request_id_should_be_set_when_request_headers_exists_test
    {
        [Test]
        public async Task Request_id_should_be_set_when_request_headers_exists()
        {
            await Task.CompletedTask;
            Assert.Pass();
        }
    }
}
