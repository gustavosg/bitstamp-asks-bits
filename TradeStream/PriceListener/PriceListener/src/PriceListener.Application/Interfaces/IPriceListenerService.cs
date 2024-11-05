namespace PriceListener.Application.Interfaces
{
    public interface IPriceListenerService
    {
        Task DisplayInfoAsync();
        Task MonitorAndSaveEnqueuedDataAsync();
        Task StartReceivingCryptoDataAsync();
        Task StopReceivingCryptoDataAsync();
    }
}
