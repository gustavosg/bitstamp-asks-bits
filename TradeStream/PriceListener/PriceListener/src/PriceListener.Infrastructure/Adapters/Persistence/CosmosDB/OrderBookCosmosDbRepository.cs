using PriceListener.Domain.Entities.Bitstamp;
using PriceListener.Domain.Interfaces.Persistence;
using PriceListener.Domain.Interfaces.Persistence.CosmosDB;
using Microsoft.Azure.Cosmos;

namespace PriceListener.Infrastructure.Adapters.Persistence.CosmosDB
{
    public class OrderBookCosmosDbRepository : IOrderBookCosmosDbRepository
    {
        private readonly ICosmosDbContext cosmosDbContext;

        public OrderBookCosmosDbRepository(ICosmosDbContext cosmosDBContext)
        {
            cosmosDbContext = cosmosDBContext;
        }

        public async Task<OrderBook> GetOrderBookAsync(string id, string partitionKey)
        {
            return await cosmosDbContext.Get<OrderBook>(id, partitionKey);
        }

        public async Task<IEnumerable<OrderBook>> GetAllOrderBookAsync(string partitionKey)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.PartitionKey = @partitionKey")
                .WithParameter("@partitionKey", partitionKey);

            var items = new List<OrderBook>();
            using (var iterator = cosmosDbContext.GetContainer().GetItemQueryIterator<OrderBook>(query))
            {
                while (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync();
                    items.AddRange(response);
                }
            }

            return items;
        }

        public async Task<OrderBook> CreateOrderBookAsync(OrderBook orderBookData)
            => await cosmosDbContext.Add(orderBookData);

        public async Task UpdateOrderBookAsync(OrderBook orderBookData)
            => await cosmosDbContext.Update(orderBookData.Id.ToString(), orderBookData);

        public async Task DeleteOrderBookAsync(OrderBook orderBook)
            => await cosmosDbContext.Delete<OrderBook>(orderBook);
    }

}