using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tests.HttpTestServerUtilities
{
    public class Serializer
    {
        public static JsonSerializerOptions SerializerOptions =>
#if NETCOREAPP3_1
            DefaultSerializerOptions;
#else
            new JsonSerializerOptions(JsonSerializerDefaults.Web);
#endif

        internal static readonly JsonSerializerOptions DefaultSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            //NumberHandling = Serialization.JsonNumberHandling.AllowReadingFromString,

            // Web defaults don't use the relex JSON escaping encoder.
            //
            // Because these options are for producing content that is written directly to the request
            // (and not embedded in an HTML page for example), we can use UnsafeRelaxedJsonEscaping.
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };
    }
}
