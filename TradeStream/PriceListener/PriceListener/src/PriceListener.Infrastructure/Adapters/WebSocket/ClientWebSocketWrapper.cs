using PriceListener.Domain.Interfaces.Adapters.WebSocket;
using System.Net.WebSockets;

namespace PriceListener.Infrastructure.Adapters.WebSocket
{
    public class ClientWebSocketWrapper : IClientWebSocketWrapper
    {
        private readonly ClientWebSocket clientWebSocket;

        public ClientWebSocketWrapper()
        {
            this.clientWebSocket = new ClientWebSocket();
        }

        public WebSocketState State => this.clientWebSocket.State;

        public Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
        {
            return this.clientWebSocket.ReceiveAsync(buffer, cancellationToken);
        }

        public Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            return this.clientWebSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
        }

        public Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        => this.clientWebSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
        

        public void Dispose()
        {
            this.clientWebSocket.Dispose();
        }

        public async Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
        => await this.clientWebSocket.ConnectAsync(uri, cancellationToken);
    }

}
