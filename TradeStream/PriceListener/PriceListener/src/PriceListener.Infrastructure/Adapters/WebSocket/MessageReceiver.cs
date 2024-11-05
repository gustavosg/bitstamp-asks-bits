using PriceListener.Domain.Interfaces.Adapters.WebSocket;
using System.Net.WebSockets;
using System.Text;

namespace PriceListener.Infrastructure.Adapters.WebSocket
{
    public class MessageReceiver : IMessageReceiver
    {
        private const string MSG_CANCELLATION_REQUESTED = "Cancellation Requested";
        private const string MSG_DISCONNECTED = "Disconnected from server";
        private const string MSG_WEBSOCKET_CLOSED = "Websocket closed";

        private readonly IClientWebSocketWrapper clientWebSocket;
        private CancellationToken cancellationToken;

        public event Action<string> OnMessageReceived;

        public MessageReceiver(
            IWebSocketConnector connector,
            CancellationTokenSource cancellationTokenSource
        )
        {
            this.clientWebSocket = connector.GetClientWebSocket();
            this.cancellationToken = cancellationTokenSource.Token;
        }

        public async Task ReceiveDataAsync()
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;

            while (true)
            {
                StringBuilder msgBuilder = new();

                if (this.clientWebSocket.State != WebSocketState.Open)
                {
                    Console.WriteLine(MSG_WEBSOCKET_CLOSED);
                    return;
                }

                do
                {
                    result = await this.clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), this.cancellationToken);
                    string messagePart = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    msgBuilder.Append(messagePart);

                } while (!result.EndOfMessage);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = msgBuilder.ToString();
                    OnMessageReceived?.Invoke(message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await this.clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, MSG_DISCONNECTED, this.cancellationToken);
                    Console.WriteLine(MSG_DISCONNECTED);
                    break; 
                }
            }
        }
    }
}
