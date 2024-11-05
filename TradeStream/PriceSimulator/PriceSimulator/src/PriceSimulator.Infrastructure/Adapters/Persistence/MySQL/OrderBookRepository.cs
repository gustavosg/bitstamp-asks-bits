using Microsoft.EntityFrameworkCore;
using PriceSimulator.Domain.Entities;
using PriceSimulator.Domain.Entities.Bitstamp;
using PriceSimulator.Domain.Helpers;
using PriceSimulator.Domain.Interfaces.Persistence.MySQL;

namespace PriceSimulator.Infrastructure.Adapters.Persistence.MySQL
{
    public class OrderBookRepository : IOrderBookRepository
    {
        private readonly AppDbContext appDbContext;
        public DbSet<OrderBook> DbSet { get; set; }

        public OrderBookRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            this.DbSet = appDbContext.Set<OrderBook>();
        }

        public async Task Add(OrderBook orderBook)
        {
            await this.appDbContext.AddAsync(orderBook);

            await this.appDbContext.SaveChangesAsync();
        }

        public async Task<OrderBook> GetLatestOrderBook(Cryptocurrency cryptocurrency)
        => await (from ob in this.DbSet
                    .Include(x => x.Data)
                       .ThenInclude(x => x.Asks)
                    .Include(x => x.Data)
                        .ThenInclude(x => x.Bids)
                    .AsNoTracking()
                  where string.Equals(ob.Channel, cryptocurrency.CryptocurrencyToSubscribeChannel())
                  orderby ob.Data.Timestamp descending
                  select ob).FirstOrDefaultAsync() ?? new();
    }
}
