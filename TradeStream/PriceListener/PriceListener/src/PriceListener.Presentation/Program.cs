using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PriceListener.Application.Interfaces;
using PriceListener.Application.Services;
using PriceListener.Infrastructure.Adapters.API.Bitstamp.DependencyConfiguration;
using PriceListener.Infrastructure.Adapters.WebSocket.DependencyConfiguration;
using PriceListener.Presentation.Terminal;
using PriceListener.Infrastructure.Adapters.Persistence.CosmosDB.DependencyConfiguration;
using PriceListener.Infrastructure.Adapters.Persistence.MySQL.DependencyConfiguration;
using Microsoft.Extensions.Logging;
using PriceListener.Domain.Interfaces.Services.PriceListener;
using PriceListener.Domain.Services.PriceListener;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal class Program
{
    private static readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    private static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        WorkerService worker = new WorkerService(
            host.Services.GetService<IPriceListenerService>(),
            cancellationTokenSource
            );

        await worker.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddMySqlServices();
                services.AddSingleton(cancellationTokenSource);
                services.AddBitstampServices();
                services.AddWebSocketServices();
                services.AddCosmosDbServices();
                services.AddCosmosDbRepository();
                services.AddDbRepositories();

                services.AddScoped<IDataProcessor, DataProcessor>();
                services.AddScoped<IOrderBookStatisticsService, OrderBookStatisticsService>();
                services.AddScoped<IPriceListenerService, PriceListenerService>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            });
    }
}