using Microsoft.Extensions.DependencyInjection;
using PriceListener.Domain.Interfaces.Adapters.WebSocket;

namespace PriceListener.Infrastructure.Adapters.WebSocket.DependencyConfiguration
{
    public static class DependencyConfiguration
    {
        public static IServiceCollection AddWebSocketServices(this IServiceCollection services)
        {
            services.AddSingleton<IMessageReceiver, MessageReceiver>();
            services.AddSingleton<IMessageSender, MessageSender>();
            services.AddSingleton<IWebSocketConnector, WebSocketConnector>();
            services.AddSingleton<IWebSocketAdapter, WebSocketAdapter>();
            services.AddSingleton<IClientWebSocketWrapper, ClientWebSocketWrapper>();

            return services;
        }
    }
}
