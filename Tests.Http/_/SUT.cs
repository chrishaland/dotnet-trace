using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tests.Http
{
    internal class SUT
    {
        internal static HttpClient Client => TestServerSetup.Client;
        internal static JsonSerializerOptions SerializerOptions =>
#if NETCOREAPP3_1
            Microsoft.AspNetCore.Http.Json.JsonOptions.DefaultSerializerOptions;
#else
            new JsonSerializerOptions(JsonSerializerDefaults.Web);
#endif

        internal static HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, string path) => new HttpRequestMessage(method, path);

        internal static async Task<TraceMetadataResponse> GetTraces(Action<HttpRequestMessage> requestOptions = null)
        {
            var request = CreateHttpRequestMessage(HttpMethod.Get, "/traces");
            if (requestOptions != null) requestOptions(request);

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            return await JsonSerializer.DeserializeAsync<TraceMetadataResponse>(contentStream, SerializerOptions);
        }
    }
}
