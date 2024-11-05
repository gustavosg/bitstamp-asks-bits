using Microsoft.EntityFrameworkCore;
using Moq;
using PriceListener.Domain.Entities.Bitstamp;
using PriceListener.Infrastructure.Adapters.Persistence.MySQL;

namespace PriceListener.Tests.Infrastructure.Adapters.Persistence.MySQL
{
    public class OrderBookRepositoryTest
    {
        private readonly Mock<AppDbContext> contextMock;
        private readonly OrderBookRepository repository;

        public OrderBookRepositoryTest()
        {
            this.contextMock = new Mock<AppDbContext>();
            this.repository = new OrderBookRepository(this.contextMock.Object);
        }

        [Fact]
        public async Task Add_ShouldAddOrderBook()
        {
            // Arrange
            var orderBook = new OrderBook { Id = Guid.NewGuid() };
            this.contextMock.Setup(m => m.Set<OrderBook>()).Returns(new Mock<DbSet<OrderBook>>().Object);

            // Act
            await this.repository.Add(orderBook);

            // Assert
            this.contextMock.Verify(m => m.AddAsync(orderBook, default), Times.Once);
            this.contextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }
    }
}

