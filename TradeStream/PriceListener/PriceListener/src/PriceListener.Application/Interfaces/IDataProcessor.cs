using PriceListener.Domain.Entities.Bitstamp;
using System.Collections.Concurrent;

namespace PriceListener.Application.Interfaces
{
    public interface IDataProcessor
    {
        void EnqueueData(OrderBook book);
        BlockingCollection<OrderBook> GetQueue();
    }
}
