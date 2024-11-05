using Base.Converteres;
using PriceListener.Domain.Converters;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PriceListener.Domain.Entities.Bitstamp
{
    [Table("OrderBookDatas")]
    public class OrderBookData : BaseEntity
    {
        [JsonPropertyName("timestamp")]
        [JsonConverter(typeof(UnixTimeSecondsConverter))]
        public DateTimeOffset Timestamp { get; set; }
        [JsonPropertyName("microtimestamp")]
        [JsonConverter(typeof(UnixTimeMicrosecondsConverter))]
        public DateTimeOffset Microtimestamp { get; set; }
        [JsonPropertyName("bids")]
        [JsonConverter(typeof(CurrencyPriceConverter<Bids>))]
        public List<Bids> Bids { get; set; }
        [JsonPropertyName("asks")]
        [JsonConverter(typeof(CurrencyPriceConverter<Asks>))]
        public List<Asks> Asks { get; set; }
    }
}
