using PriceListener.Domain.Interfaces.Persistence.MySQL;

namespace PriceListener.Infrastructure.Adapters.Persistence.MySQL
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext appDbContext;
        private IOrderBookRepository orderBookRepository;

        public UnitOfWork(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IOrderBookRepository OrderBookRepository
            => this.orderBookRepository ?? new OrderBookRepository(this.appDbContext);

        public void Dispose()
        {
            this.appDbContext.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        => await this.appDbContext.SaveChangesAsync();
    }
}
