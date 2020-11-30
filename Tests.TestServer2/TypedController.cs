using System.Threading.Tasks;
using Haland.DotNetTrace;
using Microsoft.AspNetCore.Mvc;

namespace Tests.TestServer2
{
    [Route("typed")]
    public class TypedController : ControllerBase
    {
        private readonly TypedHttpClient _client;

        public TypedController(TypedHttpClient httpClient, TraceMetadata traces)
        {
            _client = httpClient;
        }

        [HttpGet]
        [HttpPost]
        public async Task<ActionResult<TraceMetadata>> Traces()
        {
            var traces = await _client.GetTraces();
            return new JsonResult(traces);
        }
    }
}
