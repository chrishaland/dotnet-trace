using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Haland.DotNetTrace;
using Microsoft.AspNetCore.Mvc;
using Tests.HttpTestServerUtilities;

namespace Tests.HttpTestServer2
{
    [Route("basic")]
    public class BasicController : ControllerBase
    {
        private readonly HttpClient _client;

        public BasicController(IHttpClientFactory clientFactory, TraceMetadata traces)
        {
            _client = clientFactory.CreateClient();
        }

        [HttpGet]
        [HttpPost]
        public async Task<ActionResult<TraceMetadata>> Traces()
        {
            var traces = await GetTraces();
            return new JsonResult(traces);
        }

        private async Task<TraceMetadataResponse> GetTraces()
        {
            var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/"));
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            return await JsonSerializer.DeserializeAsync<TraceMetadataResponse>(contentStream, Serializer.SerializerOptions);
        }
    }
}
