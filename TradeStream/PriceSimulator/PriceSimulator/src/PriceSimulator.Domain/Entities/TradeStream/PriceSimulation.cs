using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PriceSimulator.Domain.Entities.Bitstamp;

namespace PriceSimulator.Domain.Entities.TradeStream
{
    [Table("PriceSimulations")]
    public class PriceSimulation : BaseEntity
    {
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }
        [JsonPropertyName("cryptocurrency")]
        public Cryptocurrency Cryptocurrency { get; set; }
        [JsonPropertyName("operationType")]
        public OperationType OperationType { get; set; }
        [JsonPropertyName("totalCost")]
        public decimal TotalCost { get; set; }
        [JsonPropertyName("pricesUsed")]
        public List<Price> PricesUsed { get; set; } = new();
    }
}
