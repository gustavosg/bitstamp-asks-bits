using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PriceSimulator.Domain.Entities.Bitstamp
{
    [Table("OrderBookDatas")]
    public class OrderBookData : BaseEntity
    {
        [JsonPropertyName("timestamp")]
        public DateTimeOffset Timestamp { get; set; }
        [JsonPropertyName("microtimestamp")]
        public DateTimeOffset Microtimestamp { get; set; }
        [JsonPropertyName("bids")]
        public List<Bids> Bids { get; set; }
        [JsonPropertyName("asks")]
        public List<Asks> Asks { get; set; }
    }
}
