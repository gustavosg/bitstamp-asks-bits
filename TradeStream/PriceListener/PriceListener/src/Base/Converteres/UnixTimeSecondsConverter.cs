using System.Text.Json;
using System.Text.Json.Serialization;

namespace Base.Converteres
{
    public class UnixTimeSecondsConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            long timestamp = Int64.Parse(reader.GetString());
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).ToUniversalTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.ToUnixTimeSeconds());
        }
    }
}
