using PriceListener.Domain.Entities.Bitstamp;

namespace PriceListener.Domain.Interfaces.Persistence.MySQL
{
    public interface IOrderBookRepository
    {
        Task Add(OrderBook orderBook);
    }
}
