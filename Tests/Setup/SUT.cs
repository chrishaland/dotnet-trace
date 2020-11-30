using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tests.Utilities;

namespace Tests
{
    internal class SUT
    {
        internal static HttpClient HttpClient1 => TestServerSetup.TestServer1HttpClient;
        internal static GrpcTracer.Tracer.TracerClient GrpcClient1 => TestServerSetup.TestServer1GrpcClient;
        internal static HttpClient HttpClient2 => TestServerSetup.TestServer2HttpClient;
        internal static GrpcTracer.Tracer.TracerClient GrpcClient2 => TestServerSetup.TestServer2GrpcClient;

        internal static async Task<TraceMetadataResponse> SendHttpRequest(HttpRequestMessage request, HttpClient client)
        {
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            return await JsonSerializer.DeserializeAsync<TraceMetadataResponse>(contentStream, Serializer.SerializerOptions);
        }
    }
}
