using PriceSimulator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PriceSimulator.Application.DTOs
{
    public class PriceSimulationRequest
    {
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }
        [JsonPropertyName("cryptocurrency")]
        public Cryptocurrency Cryptocurrency { get; set; }
        [JsonPropertyName("operationType")]
        public OperationType OperationType { get; set; }
    }
}
