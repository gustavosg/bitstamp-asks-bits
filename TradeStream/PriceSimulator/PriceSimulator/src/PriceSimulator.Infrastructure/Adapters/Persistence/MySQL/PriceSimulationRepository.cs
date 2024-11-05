using Microsoft.EntityFrameworkCore;
using PriceSimulator.Domain.Entities.TradeStream;
using PriceSimulator.Domain.Interfaces.Persistence.MySQL;

namespace PriceSimulator.Infrastructure.Adapters.Persistence.MySQL
{
    public class PriceSimulationRepository : IPriceSimulationRepository
    {
        private readonly AppDbContext appDbContext;
        public DbSet<PriceSimulation> DbSet { get; set; }

        public PriceSimulationRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            this.DbSet = appDbContext.Set<PriceSimulation>();
        }

        public async Task<PriceSimulation> Add(PriceSimulation priceSimulation)
        {
            await this.appDbContext.AddAsync(priceSimulation);
            await this.appDbContext.SaveChangesAsync();

            return priceSimulation;
        }

        public async Task<bool> Exists(Guid id)
            => await this.DbSet.AnyAsync(x => x.Id.Equals(id));
        public async Task<PriceSimulation> Get(Guid id)
            => await this.DbSet
            .Include(x => x.PricesUsed).AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(id));

        public async Task<List<PriceSimulation>> Get()
        => await (
            from ps in this.DbSet.Include(x => x.PricesUsed).AsNoTracking()
            select ps
            ).ToListAsync();
    }
}
