using System.Text.Json.Serialization;

namespace PriceSimulator.Domain.Entities.Bitstamp
{
    public class SubscribeData
    {
        [JsonPropertyName("channel")]
        public string Channel { get; set; }
    }
}
