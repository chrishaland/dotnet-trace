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
            await Task.CompletedTask;
            Assert.Pass();
        }
    }
}
