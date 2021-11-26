namespace Tests.TestServer2;

public class TypedHttpClient
{
    public HttpClient Client { get; }

    public TypedHttpClient(HttpClient client)
    {
        client.BaseAddress = new Uri("https://localhost:5000");
        Client = client;
    }

    public async Task<TraceMetadataResponse> GetTraces()
    {
        var response = await Client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/"));
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        return await JsonSerializer.DeserializeAsync<TraceMetadataResponse>(contentStream, Serializer.SerializerOptions);
    }
}
