using PriceSimulator.Domain.Entities.Bitstamp;
using PriceSimulator.Domain.Entities.TradeStream;
using PriceSimulator.Domain.Services1;

namespace PriceSimulator.Domain.Interfaces.Services
{
    public interface IPriceSimulationService
    {
        PriceSimulation CalculateBestPriceForBuy(IEnumerable<Asks> asks, decimal quantity);
        PriceSimulation CalculateBestPriceForSell(IEnumerable<Bids> bids, decimal quantity);
    }
}
