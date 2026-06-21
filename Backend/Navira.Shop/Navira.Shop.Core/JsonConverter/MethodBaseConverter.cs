using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Navira.Shop.Core
{
    public class MethodBaseConverter : JsonConverter<MethodBase>
    {
        // Determines if this converter can handle the type.
        // This is crucial for inheriting types like MethodInfo.
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(MethodBase).IsAssignableFrom(typeToConvert);
        }

        // Prevents the serialization of any MethodBase instance.
        public override void Write(Utf8JsonWriter writer, MethodBase value, JsonSerializerOptions options)
        {
            // Write a null value to the JSON output.
            writer.WriteNullValue();
        }

        // Prevents the deserialization of MethodBase.
        public override MethodBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Return null for any MethodBase deserialization attempt.
            return null;
        }
    }
}
