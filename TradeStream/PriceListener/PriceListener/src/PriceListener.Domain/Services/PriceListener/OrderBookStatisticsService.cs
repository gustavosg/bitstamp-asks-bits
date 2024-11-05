using PriceListener.Domain.Entities.Bitstamp;
using PriceListener.Domain.Interfaces.Services.PriceListener;

namespace PriceListener.Domain.Services.PriceListener
{
    public class OrderBookStatisticsService : IOrderBookStatisticsService
    {
        public decimal GetAveragePriceAsksOverLastFiveSeconds(IEnumerable<OrderBook> orderBooks)
        => (
            from ob in orderBooks
            where ob.Data.Timestamp >= 
                orderBooks.OrderByDescending(x => x.Data.Timestamp)?.FirstOrDefault()
                .Data.Timestamp.ToUniversalTime().AddSeconds(-5)
            select ob.Data.Asks.Average(x => x.Price)
            )
            .FirstOrDefault();
        public decimal GetAveragePriceBidsOverLastFiveSeconds(IEnumerable<OrderBook> orderBooks)
        => (
            from ob in orderBooks
            where ob.Data.Timestamp >= 
                orderBooks.OrderByDescending(x => x.Data.Timestamp)?.FirstOrDefault()
                .Data.Timestamp.ToUniversalTime().AddSeconds(-5)
            select ob.Data.Bids.Average(x => x.Price)
            )
            .FirstOrDefault();

        public decimal GetAverageQuantity(List<CurrencyPrice> prices)
            => prices.Average(x => x.Amount);

        public decimal GetAveragePrice(List<CurrencyPrice> prices)
            => prices.Average(x => x.Price);

        public decimal GetMaxPrice(List<CurrencyPrice> prices)
            => prices.Max(x => x.Price);

        public decimal GetMinPrice(List<CurrencyPrice> prices)
            => prices.Min(x => x.Price);
    }
}
