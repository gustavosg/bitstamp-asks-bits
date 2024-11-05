using System.Net.WebSockets;

namespace PriceSimulator.Domain.Interfaces.Adapters.WebSocket
{
    public interface IWebSocketConnector
    {
        Task ConnectAsync(string uri);
        Task DisconnectAsync();
        IClientWebSocketWrapper GetClientWebSocket();
    }
}
