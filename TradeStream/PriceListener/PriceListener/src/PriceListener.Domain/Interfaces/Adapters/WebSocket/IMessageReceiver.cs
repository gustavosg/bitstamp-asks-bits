namespace PriceListener.Domain.Interfaces.Adapters.WebSocket
{
    public interface IMessageReceiver
    {
        event Action<string> OnMessageReceived;
        Task ReceiveDataAsync();
    }
}
