using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tests.HttpTestServerUtilities;

namespace Tests.Http
{
    internal class SUT
    {
        internal static HttpClient TestServer1 => TestServerSetup.TestServer1;
        internal static HttpClient TestServer2 => TestServerSetup.TestServer2;
        
        internal static async Task<TraceMetadataResponse> SendRequest(HttpRequestMessage request, HttpClient client)
        {
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            return await JsonSerializer.DeserializeAsync<TraceMetadataResponse>(contentStream, Serializer.SerializerOptions);
        }
    }
}
