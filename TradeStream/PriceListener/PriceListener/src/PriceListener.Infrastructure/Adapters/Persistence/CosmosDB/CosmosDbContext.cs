using Microsoft.Azure.Cosmos;
using PriceListener.Domain.Interfaces.Persistence.CosmosDB;

namespace PriceListener.Infrastructure.Adapters.Persistence.CosmosDB
{
    public class CosmosDbContext : ICosmosDbContext
    {
        private readonly CosmosClient _cosmosClient;
        private readonly Database _database;
        private readonly Container _container;

        public CosmosDbContext(string accountEndpoint, string authKey, string databaseId, string containerId)
        {
            _cosmosClient = new CosmosClient(accountEndpoint, authKey, new CosmosClientOptions()
            {
                   SerializerOptions = new CosmosSerializationOptions()
                   {
                       PropertyNamingPolicy = CosmosPropertyNamingPolicy.Default
                   }
            })
            ;
            _database = _cosmosClient.GetDatabase(databaseId);
            _container = _database.GetContainer(containerId);
        }

        public async Task<T> Add<T>(T item) where T : class
        {
            try
            {
                var response = await _container.CreateItemAsync(item, new PartitionKey(GetPartitionKeyValue(item)));
                return response.Resource;
            }
            catch (CosmosException ex)
            {
                // Não estou preocupado com erros de throughtput, pois o intuito é apenas mostrar conhecimento sobre CosmosDB
                
                return null;
            }
        }

        public Container GetContainer() => _container;

        public async Task<T> Get<T>(string id, string partitionKey) where T : class
        {
            try
            {
                var response = await _container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Item with id {id} not found.");
                return null;
            }
        }

        public async Task<IEnumerable<T>> Get<T>(string queryString) where T : class
        {
            var query = _container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
            var results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task Update<T>(string id, T item) where T : class
        {
            try
            {
                await _container.ReplaceItemAsync(item, id, new PartitionKey(GetPartitionKeyValue(item)));
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error updating item: {ex}");
                throw;
            }
        }

        public async Task Delete<T>(T item) where T : class
        {
            try
            {
                await _container.DeleteItemAsync<T>(item.ToString(), new PartitionKey(GetPartitionKeyValue(item)));
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error deleting item: {ex}");
                throw;
            }
        }

        private static string GetPartitionKeyValue<T>(T item)
        {
            return typeof(T).GetProperty("Id")?.GetValue(item)?.ToString();
        }
    }
}
