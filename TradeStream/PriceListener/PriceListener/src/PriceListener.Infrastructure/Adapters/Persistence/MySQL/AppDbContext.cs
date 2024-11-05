using Microsoft.EntityFrameworkCore;
using PriceListener.Domain.Entities.Bitstamp;

namespace PriceListener.Infrastructure.Adapters.Persistence.MySQL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<OrderBook> OrderBook { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
