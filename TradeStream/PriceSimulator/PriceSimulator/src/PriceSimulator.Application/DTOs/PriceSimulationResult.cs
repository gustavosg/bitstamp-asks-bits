using PriceSimulator.Domain.Entities.Bitstamp;
using PriceSimulator.Domain.Entities.TradeStream;
using System.Text.Json.Serialization;

namespace PriceSimulator.Application.DTOs
{
    public class PriceSimulationResult : PriceSimulationRequest
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("totalCost")]
        public decimal TotalCost { get; set; }
        [JsonPropertyName("pricesUsed")]
        public List<CurrencyPrice> PricesUsed { get; set; }

        public PriceSimulationResult FromPriceSimulation(PriceSimulation priceSimulation)
        => new()
            {
                Id = priceSimulation.Id,
                Cryptocurrency = priceSimulation.Cryptocurrency,
                OperationType = priceSimulation.OperationType,
                PricesUsed = priceSimulation.PricesUsed.ToList<CurrencyPrice>(),
                Quantity = priceSimulation.Quantity,
                TotalCost = priceSimulation.TotalCost,
            };
        

        public PriceSimulation ToPriceSimulation()
        {
            PriceSimulation priceSimulation = new PriceSimulation();
            priceSimulation.Id = Id;
            priceSimulation.TotalCost = TotalCost;
            priceSimulation.OperationType = OperationType;
            priceSimulation.Cryptocurrency = Cryptocurrency;
            priceSimulation.Quantity = Quantity;
            priceSimulation.PricesUsed = (from pu in PricesUsed
                                              select new Price()
                                              {
                                                  Id = pu.Id,
                                                  Price = pu.Price,
                                                  Amount = pu.Amount,
                                              })
                                        .ToList();

            return priceSimulation;
        }
    }
}
