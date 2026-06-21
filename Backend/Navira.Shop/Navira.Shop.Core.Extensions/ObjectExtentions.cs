using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Navira.Shop.Core.Extensions
{
    public static class ObjectExtentions
    {
        public static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        public static string Serialize(this object dto)
        {
            return Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(dto, JsonSerializerOptions));
        }

    }
}
