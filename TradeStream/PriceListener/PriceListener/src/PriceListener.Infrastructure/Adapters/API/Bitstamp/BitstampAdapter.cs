using PriceListener.Domain.Entities;
using PriceListener.Domain.Entities.Bitstamp;
using PriceListener.Domain.Interfaces.Adapters.API.Bitstamp;
using PriceListener.Domain.Interfaces.Adapters.WebSocket;
using System.Text.Json;

namespace PriceListener.Infrastructure.Adapters.API.Bitstamp
{
    public class BitstampAdapter : IBitstampAdapter
    {
        private const string URI = "wss://ws.bitstamp.net/";

        private readonly IWebSocketAdapter webSocketAdapter;
        private readonly CancellationToken cancellationToken;

        public BitstampAdapter(
            IWebSocketAdapter webSocketAdapter,
            CancellationTokenSource cancellationTokenSource
            )
        {
            this.webSocketAdapter = webSocketAdapter;
            this.cancellationToken = cancellationTokenSource.Token;
        }

        public event Action<string> OnMessageReceived;

        public async Task Subscribe(List<Cryptocurrency> cryptocurrencies)
        {
            this.webSocketAdapter.OnMessageReceived += OnMessageReceived;
            await this.webSocketAdapter.ConnectAsync(URI);

            cryptocurrencies.ForEach(async cryptocurrency =>
            {
                Subscribe subscribeMessage = new Subscribe().SubscribeToChannel(cryptocurrency);
                string contentToSend = JsonSerializer.Serialize(subscribeMessage);

                await this.webSocketAdapter.SendMessageAsync(contentToSend);
            });
        }

        public async Task Unsubscribe(List<Cryptocurrency> cryptocurrencies)
        {
            cryptocurrencies.ForEach(async cryptocurrency =>
            {
                Subscribe subscribeMessage = new Subscribe().UnsubscribeToChannel(cryptocurrency);
                string contentToSend = JsonSerializer.Serialize(subscribeMessage);

                await this.webSocketAdapter.SendMessageAsync(contentToSend);
            });

            await this.webSocketAdapter.DisconnectAsync();
        }

        public async Task ReceiveMessagesAsync()
        {
            await this.webSocketAdapter.ReceiveMessageAsync();
        }
    }
}
