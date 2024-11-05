using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PriceSimulator.Domain.Entities.Bitstamp
{
    [Table("OrderBooks")]
    public class OrderBook : OrderBookBase
    {
        [JsonPropertyName("data")]
        public OrderBookData Data { get; set; }
    }
}
