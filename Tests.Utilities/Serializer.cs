using System.Text.Json;

namespace Tests.Utilities;

public static class Serializer
{
    public static JsonSerializerOptions SerializerOptions => new(JsonSerializerDefaults.Web);
}
