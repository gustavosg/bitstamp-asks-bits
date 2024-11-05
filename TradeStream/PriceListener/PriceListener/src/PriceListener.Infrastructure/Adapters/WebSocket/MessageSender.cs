using System.Text;
using System.Net.WebSockets;
using PriceListener.Domain.Interfaces.Adapters.WebSocket;

namespace PriceListener.Infrastructure.Adapters.WebSocket
{
    public class MessageSender : IMessageSender
    {
        private readonly CancellationToken cancellationToken;
        private readonly IClientWebSocketWrapper clientWebSocket;

        public MessageSender(
            IWebSocketConnector webSocketConnector,
            CancellationTokenSource cancellationTokenSource)
        {
            this.cancellationToken = cancellationTokenSource.Token;
            this.clientWebSocket = webSocketConnector.GetClientWebSocket();
        }

        public async Task SendMessageAsync(string message)
        {
            var messageBuffer = Encoding.UTF8.GetBytes(message);
            var bufferSegment = new ArraySegment<byte>(messageBuffer);

            this.cancellationToken.ThrowIfCancellationRequested();

            await this.clientWebSocket.SendAsync(
                bufferSegment,
                WebSocketMessageType.Text,
                endOfMessage: true,
                cancellationToken
            );
        }
    }
}
