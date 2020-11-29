using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tests.HttpTestServerUtilities;

namespace Tests.HttpTestServer2
{
    public class TypedHttpClient
    {
        public HttpClient Client { get; }

        public TypedHttpClient(HttpClient client)
        {
            client.BaseAddress = new Uri("http://localhost:5000");
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
}
