using PriceListener.Domain.Helpers;
using PriceListener.Application.DTOs;
using PriceListener.Application.Interfaces;
using PriceListener.Domain.Entities;
using PriceListener.Domain.Entities.Bitstamp;
using PriceListener.Domain.Interfaces.Adapters.API.Bitstamp;
using PriceListener.Domain.Interfaces.Persistence.MySQL;
using PriceListener.Domain.Interfaces.Services.PriceListener;
using System.Collections.Concurrent;
using System.Text.Json;
using PriceListener.Domain.Interfaces.Persistence.CosmosDB;

namespace PriceListener.Application.Services
{
    public class PriceListenerService : IPriceListenerService
    {
        private readonly IBitstampAdapter bitstampAdapter;
        private readonly IOrderBookCosmosDbRepository orderBookCosmosDbRepository;
        private readonly IOrderBookRepository orderBookRepository;
        private readonly IOrderBookStatisticsService orderBookStatisticsService;
        private readonly IDataProcessor dataProcessor;
        private readonly CancellationToken cancellationToken;
        private List<Cryptocurrency> cryptocurrencies = new();
        private readonly BlockingCollection<OrderBook> dataQueue;
        private List<OrderBook> orderBooks = new();

        public PriceListenerService(
            IBitstampAdapter subscribeAdapter,
            IOrderBookCosmosDbRepository orderBookCosmosDbRepository,
            IOrderBookRepository orderBookRepository,
            IOrderBookStatisticsService orderBookStatisticsService,
            IDataProcessor dataProcessor,
            CancellationTokenSource cancellationTokenSource
            )
        {
            this.bitstampAdapter = subscribeAdapter;
            this.orderBookCosmosDbRepository = orderBookCosmosDbRepository;
            this.orderBookRepository = orderBookRepository;
            this.orderBookStatisticsService = orderBookStatisticsService;
            this.dataProcessor = dataProcessor;
            this.bitstampAdapter.OnMessageReceived += EnqueueDataReceivedAsync;
            this.cancellationToken = cancellationTokenSource.Token;

            this.dataQueue = dataProcessor.GetQueue();

            this.cryptocurrencies = new()
            {
                Cryptocurrency.BTC,
                Cryptocurrency.ETH
            };
        }

        private async void EnqueueDataReceivedAsync(string obj)
        {
            OrderBookBase check = JsonSerializer.Deserialize<OrderBookBase>(obj);

            if (check.Event.Contains("subscription_succeeded"))
                return;

            OrderBook order = JsonSerializer.Deserialize<OrderBook>(obj);

            this.dataProcessor.EnqueueData(order);
        }

        public async Task MonitorAndSaveEnqueuedDataAsync()
        {
            await Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    this.dataQueue.TryTake(out OrderBook order);

                    if (order is null)
                        continue;

                    await this.orderBookCosmosDbRepository.CreateOrderBookAsync(order);

                    await this.orderBookRepository.Add(order);
                    this.orderBooks.Add(order);

                }
            });
        }

        public async Task StartReceivingCryptoDataAsync()
        {
            await this.bitstampAdapter.Subscribe(cryptocurrencies);
            await this.bitstampAdapter.ReceiveMessagesAsync();
        }

        public async Task StopReceivingCryptoDataAsync()
            => await this.bitstampAdapter.Unsubscribe(cryptocurrencies);

        public async Task DisplayInfoAsync()
        {
            List<OrderBook> books = new List<OrderBook>();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    books = this.orderBooks.ToList();

                    List<OrderBookStatisticsDTO> statistics = new();

                    foreach (Cryptocurrency cryptocurrency in cryptocurrencies)
                        statistics.Add(GetStatistics(books, cryptocurrency));

                    Console.Clear();
                    Console.WriteLine(DateTime.Now);
                    statistics.ForEach(statistic => Console.WriteLine(statistic.ToString()));


                    await Task.Delay(5000);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        private OrderBookStatisticsDTO GetStatistics(IEnumerable<OrderBook> books, Cryptocurrency cryptocurrency)
        {
            if (!books.Any())
                return new();

            OrderBookData orderBookData = books.ToList().LastOrDefault(x => x.Channel.Equals(cryptocurrency.CryptocurrencyToSubscribeChannel()))?.Data;

            if (orderBookData is null) return new();

            return new OrderBookStatisticsDTO(
                cryptocurrency,
                this.orderBookStatisticsService.GetMaxPrice(orderBookData.Asks.ToList<CurrencyPrice>()),
                this.orderBookStatisticsService.GetMinPrice(orderBookData.Asks.ToList<CurrencyPrice>()),
                this.orderBookStatisticsService.GetMaxPrice(orderBookData.Bids.ToList<CurrencyPrice>()),
                this.orderBookStatisticsService.GetMinPrice(orderBookData.Bids.ToList<CurrencyPrice>()),
                this.orderBookStatisticsService.GetAveragePrice(orderBookData.Asks.ToList<CurrencyPrice>()),
                this.orderBookStatisticsService.GetAveragePrice(orderBookData.Bids.ToList<CurrencyPrice>()),
                this.orderBookStatisticsService.GetAveragePriceAsksOverLastFiveSeconds(books.Where(x => x.Channel.Equals(cryptocurrency.CryptocurrencyToSubscribeChannel())).ToList()),
                this.orderBookStatisticsService.GetAveragePriceBidsOverLastFiveSeconds(books.Where(x => x.Channel.Equals(cryptocurrency.CryptocurrencyToSubscribeChannel())).ToList()),
                this.orderBookStatisticsService.GetAverageQuantity(orderBookData.Asks.ToList<CurrencyPrice>()),
                this.orderBookStatisticsService.GetAverageQuantity(orderBookData.Bids.ToList<CurrencyPrice>())
                );
        }
    }
}
