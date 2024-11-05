using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using PriceListener.Domain.Entities.Bitstamp;
using PriceListener.Infrastructure.Adapters.Persistence.CosmosDB;
using PriceListener.Domain.Interfaces.Persistence.CosmosDB;
using Microsoft.Azure.Cosmos;

namespace PriceListener.Tests.Infrastructure.Adapters.Persistence.CosmosDB
{
    public class OrderBookCosmosDbRepositoryTest
    {
        private readonly Mock<ICosmosDbContext> cosmosDbContextMock;
        private readonly OrderBookCosmosDbRepository repository;

        public OrderBookCosmosDbRepositoryTest()
        {
            this.cosmosDbContextMock = new Mock<ICosmosDbContext>();
            this.repository = new OrderBookCosmosDbRepository(cosmosDbContextMock.Object);
        }

        [Fact]
        public async Task GetOrderBookAsync_ShouldReturnOrderBook_WhenExists()
        {
            // Arrange
            var orderBookId = Guid.NewGuid();
            var partitionKey = "Id";
            var expectedOrderBook = new OrderBook { Id = orderBookId };

            this.cosmosDbContextMock.Setup(c => c.Get<OrderBook>(orderBookId.ToString(), partitionKey))
                .ReturnsAsync(expectedOrderBook);

            // Act
            var result = await this.repository.GetOrderBookAsync(orderBookId.ToString(), partitionKey);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOrderBook.Id, result.Id);
        }

        // TODO @gustavo verify error
        //[Fact]
        //public async Task GetAllOrderBookAsync_ShouldReturnAllOrderBooks()
        //{
        //    // Arrange
        //    var partitionKey = "Id";
        //    var orderBooks = new List<OrderBook>
        //    {
        //        new OrderBook { Id = Guid.NewGuid() },
        //        new OrderBook { Id = Guid.NewGuid() }
        //    };

        //    var mockIterator = new Mock<FeedIterator<OrderBook>>();

        //    mockIterator.SetupSequence(i => i.HasMoreResults)
        //        .Returns(true)   
        //        .Returns(false); 

        //    // Mock de FeedResponse
        //    var mockResponse = new Mock<FeedResponse<OrderBook>>();
        //    mockResponse.Setup(r => r.Resource).Returns(orderBooks);
        //    mockResponse.Setup(r => r.Count).Returns(orderBooks.Count);

        //    // Setup para ReadNextAsync
        //    mockIterator.Setup(i => i.ReadNextAsync(It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(mockResponse.Object);

        //    var containerMock = new Mock<Container>();
        //    containerMock.Setup(c => c.GetItemQueryIterator<OrderBook>(It.IsAny<QueryDefinition>(), null, null))
        //        .Returns(mockIterator.Object);

        //    cosmosDbContextMock.Setup(c => c.GetContainer()).Returns(containerMock.Object);

        //    // Act
        //    var result = await repository.GetAllOrderBookAsync(partitionKey);

        //    // Assert
        //    Assert.Equal(2, result.Count());
        //}

        [Fact]
        public async Task CreateOrderBookAsync_ShouldAddOrderBook()
        {
            // Arrange
            var orderBook = new OrderBook { Id = Guid.NewGuid() };

            this.cosmosDbContextMock.Setup(c => c.Add(orderBook)).ReturnsAsync(orderBook);

            // Act
            var result = await repository.CreateOrderBookAsync(orderBook);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderBook.Id, result.Id);
        }

        [Fact]
        public async Task UpdateOrderBookAsync_ShouldUpdateOrderBook()
        {
            // Arrange
            var orderBook = new OrderBook { Id = Guid.NewGuid() };

            this.cosmosDbContextMock.Setup(c => c.Update(orderBook.Id.ToString(), orderBook))
                .Returns(Task.CompletedTask);

            // Act
            await this.repository.UpdateOrderBookAsync(orderBook);

            // Assert
            this.cosmosDbContextMock.Verify(c => c.Update(orderBook.Id.ToString(), orderBook), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderBookAsync_ShouldDeleteOrderBook()
        {
            // Arrange
            OrderBook orderBook = Mock.Of<OrderBook>();

            this.cosmosDbContextMock.Setup(c => c.Delete<OrderBook>(orderBook))
                .Returns(Task.CompletedTask);

            // Act
            await this.repository.DeleteOrderBookAsync(orderBook);

            // Assert
            this.cosmosDbContextMock.Verify(c => c.Delete<OrderBook>(orderBook), Times.Once);
        }
    }
}
