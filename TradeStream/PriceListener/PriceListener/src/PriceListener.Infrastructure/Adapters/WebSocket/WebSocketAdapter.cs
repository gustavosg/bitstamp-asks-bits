using PriceListener.Domain.Interfaces.Adapters.WebSocket;

namespace PriceListener.Infrastructure.Adapters.WebSocket
{
    public class WebSocketAdapter : IWebSocketAdapter
    {
        private readonly IWebSocketConnector connector;
        private readonly IMessageReceiver receiver;
        private readonly IMessageSender sender;
        private readonly CancellationToken cancellationToken;

        public event Action<string> OnMessageReceived;

        public WebSocketAdapter(
            IWebSocketConnector connector,
            IMessageReceiver receiver,
            IMessageSender sender,
            CancellationTokenSource cancellationTokenSource
        )
        {
            this.connector = connector;
            this.receiver = receiver;
            this.sender = sender;
            this.cancellationToken = cancellationTokenSource.Token;

            this.receiver.OnMessageReceived += message => OnMessageReceived?.Invoke(message);
        }

        public async Task ConnectAsync(string uri)
        {
            await this.connector.ConnectAsync(uri);
        }

        public async Task ReceiveMessageAsync()
        {
            await this.receiver.ReceiveDataAsync();
        }

        public async Task SendMessageAsync(string message)
        {
            await this.sender.SendMessageAsync(message);
        }

        public async Task DisconnectAsync()
        {
            await this.connector.DisconnectAsync();
        }
    }
}
