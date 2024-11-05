using PriceListener.Domain.Entities;

namespace PriceListener.Domain.Interfaces.Adapters.API.Bitstamp
{
    public interface IBitstampAdapter
    {
        event Action<string> OnMessageReceived;
        Task ReceiveMessagesAsync();
        Task Subscribe(List<Cryptocurrency> cryptocurrencies);
        Task Unsubscribe(List<Cryptocurrency> cryptocurrencies);
    }
}
