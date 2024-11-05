using Microsoft.Extensions.DependencyInjection;
using PriceListener.Domain.Interfaces.Persistence;
using PriceListener.Domain.Interfaces.Persistence.CosmosDB;

namespace PriceListener.Infrastructure.Adapters.Persistence.CosmosDB.DependencyConfiguration
{
    public static class DependencyConfiguration
    {
        public static IServiceCollection AddCosmosDbServices(this IServiceCollection services)
        => services.AddSingleton<ICosmosDbContext>(provider =>
            {
                string accountEndpoint = "";
                string authKey = "";
                string databaseId = "";
                string containerId = "";
                return new CosmosDbContext(accountEndpoint, authKey, databaseId, containerId);
            });
        
        public static IServiceCollection AddCosmosDbRepository(this IServiceCollection services)
        {
            services.AddScoped<IOrderBookCosmosDbRepository, OrderBookCosmosDbRepository>();

            return services;
        }
    }
}
