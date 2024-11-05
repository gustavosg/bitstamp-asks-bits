using PriceSimulator.Application.DTOs;
using PriceSimulator.Domain.Entities;
using PriceSimulator.Domain.Entities.TradeStream;

namespace PriceSimulator.Application.Interfaces
{
    public interface IPriceService
    {
        Task<PriceSimulationResult> CalculateBestBuyPrice(PriceSimulationRequest request);
        Task<bool> Exists(Guid id);
        Task<PriceSimulationResult> Get(Guid id);
        Task<List<PriceSimulationResult>> Get();
    }
}
