using global::PriceSimulator.Domain.Entities.Bitstamp;
using global::PriceSimulator.Domain.Services1;

namespace PriceSimulator.Tests.Domain.Services.PriceSimulator

{
    public class PriceSimulationServiceTests
    {
        private readonly PriceSimulationService _priceSimulationService;

        public PriceSimulationServiceTests()
        {
            _priceSimulationService = new PriceSimulationService();
        }

        [Fact]
        public void CalculateBestPriceForBuy_ShouldCalculateCorrectTotalCostAndQuantity()
        {
            // Arrange
            var asks = new List<Asks>
            {
                new Asks { Price = 100, Amount = 1 },
                new Asks { Price = 90, Amount = 2 },
                new Asks { Price = 95, Amount = 1.5m }
            };
            decimal quantity = 2.5m;

            // Act
            var result = _priceSimulationService.CalculateBestPriceForBuy(asks, quantity);

            // Assert
            Assert.Equal(227.5m, result.TotalCost);
            Assert.Equal(2.5m, result.Quantity);
            Assert.Equal(2, result.PricesUsed.Count);
        }

        [Fact]
        public void CalculateBestPriceForSell_ShouldCalculateCorrectTotalCostAndQuantity()
        {
            // Arrange
            var bids = new List<Bids>
            {
                new Bids { Price = 110, Amount = 1 },
                new Bids { Price = 115, Amount = 2 },
                new Bids { Price = 105, Amount = 1.5m }
            };
            decimal quantity = 2.5m;

            // Act
            var result = _priceSimulationService.CalculateBestPriceForSell(bids, quantity);

            // Assert
            Assert.Equal(285.0m, result.TotalCost);
            Assert.Equal(2.5m, result.Quantity);
            Assert.Equal(2, result.PricesUsed.Count);
        }
    }
}
