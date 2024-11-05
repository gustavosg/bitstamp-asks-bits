using PriceListener.Domain.Entities.Bitstamp;

namespace PriceListener.Domain.Interfaces.Services.PriceListener
{
    public interface IOrderBookStatisticsService
    {
        decimal GetAveragePriceAsksOverLastFiveSeconds(IEnumerable<OrderBook> orderBooks);
        decimal GetAveragePriceBidsOverLastFiveSeconds(IEnumerable<OrderBook> orderBooks);
        decimal GetAverageQuantity(List<CurrencyPrice> prices);
        decimal GetAveragePrice(List<CurrencyPrice> prices);
        decimal GetMaxPrice(List<CurrencyPrice> prices);
        decimal GetMinPrice(List<CurrencyPrice> prices);
    }
}
