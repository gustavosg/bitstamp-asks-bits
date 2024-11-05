using PriceSimulator.Domain.Helpers;
using System.Text.Json.Serialization;

namespace PriceSimulator.Domain.Entities.Bitstamp
{
    public class Subscribe
    {
        [JsonPropertyName("event")]
        public string Event { get; set; }
        [JsonPropertyName("data")]
        public SubscribeData Data { get; set; }

        public Subscribe SubscribeToChannel(Cryptocurrency cryptocurrency)
        {
            Event = "bts:subscribe";
            Data = new()
            {
                Channel = cryptocurrency.CryptocurrencyToSubscribeChannel()
            };

            return this;
        }

        public Subscribe UnsubscribeToChannel(Cryptocurrency cryptocurrency)
        {
            Event = "bts:unsubscribe";
            Data = new()
            {
                Channel = cryptocurrency.CryptocurrencyToSubscribeChannel()
            };

            return this;
        }
    }
}
