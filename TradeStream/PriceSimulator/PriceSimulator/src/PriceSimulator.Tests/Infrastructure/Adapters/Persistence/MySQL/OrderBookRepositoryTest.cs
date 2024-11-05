using Microsoft.EntityFrameworkCore;
using Moq;
using PriceSimulator.Domain.Entities.Bitstamp;
using PriceSimulator.Infrastructure.Adapters.Persistence.MySQL;

namespace PriceSimulator.Tests.Infrastructure.Adapters.Persistence.MySQL
{
    public class OrderBookRepositoryTest
    {
        private readonly Mock<AppDbContext> contextMock;
        private readonly OrderBookRepository repository;

        public OrderBookRepositoryTest()
        {
            contextMock = new Mock<AppDbContext>();
            repository = new OrderBookRepository(contextMock.Object);
        }

        [Fact]
        public async Task Add_ShouldAddOrderBook()
        {
            // Arrange
            OrderBook? orderBook = new OrderBook { Id = Guid.NewGuid() };
            contextMock.Setup(m => m.Set<OrderBook>()).Returns(new Mock<DbSet<OrderBook>>().Object);

            // Act
            await repository.Add(orderBook);

            // Assert
            contextMock.Verify(m => m.AddAsync(orderBook, default), Times.Once);
            contextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }
    }
}

