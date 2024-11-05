using PriceSimulator.Domain.Entities;
using PriceSimulator.Domain.Entities.Bitstamp;

namespace PriceSimulator.Domain.Interfaces.Persistence.MySQL
{
    public interface IOrderBookRepository
    {
        Task Add(OrderBook orderBook);
        Task<OrderBook> GetLatestOrderBook(Cryptocurrency cryptocurrency);
    }
}
