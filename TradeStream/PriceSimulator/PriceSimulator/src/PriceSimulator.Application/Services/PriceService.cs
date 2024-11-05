using PriceSimulator.Application.DTOs;
using PriceSimulator.Application.Interfaces;
using PriceSimulator.Domain.Entities;
using PriceSimulator.Domain.Entities.Bitstamp;
using PriceSimulator.Domain.Entities.TradeStream;
using PriceSimulator.Domain.Interfaces.Persistence.MySQL;
using PriceSimulator.Domain.Interfaces.Services;

namespace PriceSimulator.Application.Services
{
    public class PriceService : IPriceService
    {
        private readonly IOrderBookRepository orderBookRepository;
        private readonly IPriceSimulationRepository priceSimulationRepository;
        private readonly IPriceSimulationService priceSimulationService;
        private List<OrderBook> orderBooks = new();

        public PriceService(
            IOrderBookRepository orderBookRepository,
            IPriceSimulationRepository priceSimulationRepository,
            IPriceSimulationService priceSimulationService
            )
        {
            this.orderBookRepository = orderBookRepository;
            this.priceSimulationRepository = priceSimulationRepository;
            this.priceSimulationService = priceSimulationService;
        }

        public async Task<PriceSimulationResult> CalculateBestBuyPrice(PriceSimulationRequest request)
        {
            OrderBook orderBook = await this.orderBookRepository.GetLatestOrderBook(request.Cryptocurrency);

            PriceSimulation result = request.OperationType == OperationType.Buy
                ? this.priceSimulationService.CalculateBestPriceForBuy(orderBook.Data.Asks, request.Quantity)
                : this.priceSimulationService.CalculateBestPriceForSell(orderBook.Data.Bids, request.Quantity);

            result = await this.priceSimulationRepository.Add(result);

            return new PriceSimulationResult().FromPriceSimulation(result);
        }

        public async Task<bool> Exists(Guid id)
        => await this.priceSimulationRepository.Exists(id);

        public async Task<PriceSimulationResult> Get(Guid id)
        {
            PriceSimulation priceSimulation = await this.priceSimulationRepository.Get(id);
            return new PriceSimulationResult().FromPriceSimulation(priceSimulation);
        }

        public async Task<List<PriceSimulationResult>> Get()
            => (await this.priceSimulationRepository.Get())
            .Select(x => new PriceSimulationResult().FromPriceSimulation(x))
            .ToList();
    }
}

