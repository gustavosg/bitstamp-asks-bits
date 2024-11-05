using Microsoft.EntityFrameworkCore;
using PriceListener.Domain.Entities.Bitstamp;
using PriceListener.Domain.Interfaces.Persistence.MySQL;

namespace PriceListener.Infrastructure.Adapters.Persistence.MySQL
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
    }
}
