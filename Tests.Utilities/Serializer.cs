using System.Text.Encodings.Web;
using System.Text.Json;

namespace Tests.Utilities;

public class Serializer
{
    public static JsonSerializerOptions SerializerOptions =>
#if NETCOREAPP3_1
        DefaultSerializerOptions;
#else
        new(JsonSerializerDefaults.Web);
#endif

    internal static readonly JsonSerializerOptions DefaultSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
}
