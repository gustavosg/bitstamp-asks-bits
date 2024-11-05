﻿using System.Net.WebSockets;

namespace PriceListener.Domain.Interfaces.Adapters.WebSocket
{
    public interface IWebSocketAdapter
    {
        event Action<string> OnMessageReceived;
        Task ConnectAsync(string uri);
        Task DisconnectAsync();
        Task ReceiveMessageAsync();
        Task SendMessageAsync(string message);
        
    }
}