using Microsoft.Azure.Cosmos;

namespace PriceListener.Domain.Interfaces.Persistence.CosmosDB
{
    public interface ICosmosDbContext
    {
        Task<T> Add<T>(T item) where T : class;
        //Task Delete<T>(string Id, string partitionKey) where T : class;
        Task Delete<T>(T item) where T : class;
        Container GetContainer();
        Task<T> Get<T>(string id, string partitionKey) where T : class;
        Task<IEnumerable<T>> Get<T>(string queryString) where T : class;
        Task Update<T>(string id, T item) where T : class;
    }
}
