namespace Tests.TestServer2;

[Route("named")]
public class NamedController : ControllerBase
{
    private readonly HttpClient _client;

    public NamedController(IHttpClientFactory clientFactory, TraceMetadata traces)
    {
        _client = clientFactory.CreateClient("named");
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
        var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/"));
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        return await JsonSerializer.DeserializeAsync<TraceMetadataResponse>(contentStream, Serializer.SerializerOptions);
    }
}
