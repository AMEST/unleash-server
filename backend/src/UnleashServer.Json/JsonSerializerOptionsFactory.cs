using System.Text.Json;
using System.Text.Json.Serialization;

namespace UnleashServer.Json;

public static class JsonSerializerOptionsFactory
{
    static JsonSerializerOptionsFactory()
    {
        Default = new JsonSerializerOptions();
        Default.ApplyDefaults();
    }

    public static JsonSerializerOptions Default;

    public static void ApplyDefaults(this JsonSerializerOptions opt)
    {
        opt.PropertyNameCaseInsensitive = true;
        opt.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opt.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        opt.Converters.Add(new JsonStringEnumConverter());
    }
}