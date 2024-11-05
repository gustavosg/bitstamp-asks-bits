using System.Text.Json;
using System.Text.Json.Serialization;
using PriceListener.Domain.Entities.Bitstamp;

namespace PriceListener.Domain.Converters
{
    public class CurrencyPriceConverter<T> : JsonConverter<List<T>> where T : CurrencyPrice, new()
    {
        public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var entries = new List<T>();

            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException("Expected start of array for order entries.");

            reader.Read();
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    reader.Read();
                    var price = reader.GetString();
                    reader.Read();
                    var amount = reader.GetString();
                    reader.Read();

                    entries.Add(new T
                    {
                        Amount = decimal.Parse(amount),
                        Price = decimal.Parse(price)
                    });
                }
                reader.Read();
            }

            return entries;
        }

        public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (var entry in value)
            {
                writer.WriteStartArray();
                writer.WriteStringValue(entry.Price.ToString());
                writer.WriteStringValue(entry.Amount.ToString());
                writer.WriteEndArray();
            }

            writer.WriteEndArray();
        }
    }

}
