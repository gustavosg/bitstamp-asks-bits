namespace PriceListener.Domain.Interfaces.Adapters.WebSocket
{
    public interface IMessageSender
    {
        Task SendMessageAsync(string message);
    }
}
