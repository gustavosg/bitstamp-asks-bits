using System.Net.WebSockets;
using PriceListener.Domain.Interfaces.Adapters.WebSocket;

namespace PriceListener.Infrastructure.Adapters.WebSocket
{
    public class WebSocketConnector : IWebSocketConnector
    {
        private const string MSG_DISCONNECT = "Disconnect";

        private readonly IClientWebSocketWrapper clientWebSocket;
        private readonly CancellationToken cancellationToken;

        public WebSocketConnector(
            IClientWebSocketWrapper clientWebSocket,
            CancellationTokenSource cancellationTokenSource
            )
        {
            this.cancellationToken = cancellationTokenSource.Token;
            this.clientWebSocket = clientWebSocket;
        }

        public async Task ConnectAsync(string uri)
        {
            if (!this.clientWebSocket.State.Equals(WebSocketState.Open))
                await this.clientWebSocket.ConnectAsync(new Uri(uri), cancellationToken);
        }

        public async Task DisconnectAsync()
        {
            if (this.clientWebSocket.State.Equals(WebSocketState.Open))
                await this.clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, MSG_DISCONNECT, cancellationToken);
        }
        
        public IClientWebSocketWrapper GetClientWebSocket() => this.clientWebSocket;
    }
}
