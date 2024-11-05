using PriceListener.Application.Interfaces;
using PriceListener.Domain.Entities.Bitstamp;
using System.Collections.Concurrent;

namespace PriceListener.Application.Services
{
    public class DataProcessor : IDataProcessor
    {
        private readonly BlockingCollection<OrderBook> dataQueue;

        public DataProcessor()
        {
            this.dataQueue = new BlockingCollection<OrderBook>(new ConcurrentQueue<OrderBook>());
        }
        public void EnqueueData(OrderBook book)
            => this.dataQueue.Add(book);

        public BlockingCollection<OrderBook> GetQueue() 
            => this.dataQueue;
    }
}
