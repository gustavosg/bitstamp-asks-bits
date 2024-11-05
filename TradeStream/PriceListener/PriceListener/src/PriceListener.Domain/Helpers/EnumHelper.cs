using PriceListener.Domain.Entities;

namespace PriceListener.Domain.Helpers
{
    public static class EnumHelper
    {
        public static string CryptocurrencyToSubscribeChannel(this Cryptocurrency cryptocurrency)
        => cryptocurrency switch
        {
            Cryptocurrency.BTC => "order_book_btcusd",
            Cryptocurrency.ETH => "order_book_ethusd",
            _ => throw new NotImplementedException()
        };
    }
}
