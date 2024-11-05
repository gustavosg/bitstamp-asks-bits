using PriceSimulator.Domain.Entities.Bitstamp;
using PriceSimulator.Domain.Entities.TradeStream;
using PriceSimulator.Domain.Interfaces.Services;

namespace PriceSimulator.Domain.Services1
{
    public class PriceSimulationService : IPriceSimulationService
    {
        public PriceSimulation CalculateBestPriceForBuy(IEnumerable<Asks> asks, decimal quantity)
        {
            var sortedAsks = asks.OrderBy(o => o.Price);
            return CalculateBestPrice(sortedAsks.ToList<CurrencyPrice>(), quantity);
        }

        public PriceSimulation CalculateBestPriceForSell(IEnumerable<Bids> bids, decimal quantity)
        {
            var sortedBids = bids.OrderByDescending(o => o.Price);
            return CalculateBestPrice(sortedBids.ToList<CurrencyPrice>(), quantity);
        }

        private PriceSimulation CalculateBestPrice(IEnumerable<CurrencyPrice> orders, decimal quantity)
        {
            decimal totalCost = 0;
            decimal accumulatedQuantity = 0;
            var usedOrders = new List<CurrencyPrice>();

            foreach (var order in orders)
            {
                if (accumulatedQuantity >= quantity) break;

                var quantityToUse = Math.Min(order.Amount, quantity - accumulatedQuantity);
                totalCost += quantityToUse * order.Price;
                accumulatedQuantity += quantityToUse;
                usedOrders.Add(new CurrencyPrice() { Price = order.Price, Amount = quantityToUse });
            }

            return new Domain.Entities.TradeStream.PriceSimulation
            {
                
                TotalCost = totalCost,
                Quantity = accumulatedQuantity,
                PricesUsed = usedOrders.Select(x => new Price() { Id = x.Id, Price = x.Price, Amount = x.Amount }).ToList()
            };
        }
    }
}
