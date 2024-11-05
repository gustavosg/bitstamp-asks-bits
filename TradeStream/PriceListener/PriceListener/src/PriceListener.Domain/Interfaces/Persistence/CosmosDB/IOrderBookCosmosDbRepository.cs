using PriceListener.Domain.Entities.Bitstamp;

namespace PriceListener.Domain.Interfaces.Persistence.CosmosDB
{
    public interface IOrderBookCosmosDbRepository
    {
        Task<OrderBook> CreateOrderBookAsync(OrderBook orderBook);
        Task DeleteOrderBookAsync(OrderBook orderBook);
        Task<IEnumerable<OrderBook>> GetAllOrderBookAsync(string partitionKey);
        Task<OrderBook> GetOrderBookAsync(string id, string partitionKey);
        Task UpdateOrderBookAsync(OrderBook orderBookData);
    }
}
