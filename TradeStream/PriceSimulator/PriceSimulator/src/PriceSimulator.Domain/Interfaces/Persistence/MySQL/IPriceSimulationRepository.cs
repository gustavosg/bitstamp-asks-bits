using PriceSimulator.Domain.Entities.TradeStream;

namespace PriceSimulator.Domain.Interfaces.Persistence.MySQL
{
    public interface IPriceSimulationRepository
    {
        Task<PriceSimulation> Add(PriceSimulation priceSimulation);
        Task<bool> Exists(Guid id);
        Task<PriceSimulation> Get(Guid id);
        Task<List<PriceSimulation>> Get();
    }
}
