using Microsoft.EntityFrameworkCore;
using PriceSimulator.Domain.Entities.Bitstamp;
using PriceSimulator.Domain.Entities.TradeStream;

namespace PriceSimulator.Infrastructure.Adapters.Persistence.MySQL
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
        public DbSet<PriceSimulation> PriceSimulation { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
