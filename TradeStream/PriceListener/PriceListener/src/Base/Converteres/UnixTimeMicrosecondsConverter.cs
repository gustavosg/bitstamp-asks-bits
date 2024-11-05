using System.Text.Json.Serialization;
using System.Text.Json;

namespace Base.Converteres
{
    public class UnixTimeMicrosecondsConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            long microtimestamp = Int64.Parse(reader.GetString());
            long timestampInSeconds = microtimestamp / 1_000_000;
            long microseconds = microtimestamp % 1_000_000;
            DateTimeOffset dateTime = DateTimeOffset.FromUnixTimeSeconds(timestampInSeconds).AddTicks(microseconds * 10).ToUniversalTime();

            return dateTime;
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.ToUnixTimeMilliseconds() * 1_000);
        }
    }
}
