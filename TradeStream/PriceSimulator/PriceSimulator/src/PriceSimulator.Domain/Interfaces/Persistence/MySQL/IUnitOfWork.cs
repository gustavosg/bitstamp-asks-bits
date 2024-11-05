namespace PriceSimulator.Domain.Interfaces.Persistence.MySQL
{
    public interface IUnitOfWork : IDisposable
    {
        IOrderBookRepository OrderBookRepository { get; }
        Task<int> SaveChangesAsync(); 
    }
}
