using Azure;
using Microsoft.Azure.Cosmos;
using Moq;
using PriceListener.Domain.Entities.Bitstamp;
using PriceListener.Infrastructure.Adapters.Persistence.CosmosDB;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PriceListener.Tests.Infrastructure.Adapters.Persistence.CosmosDB
{
    public class CosmosDbContextTest
    {
        private readonly Mock<CosmosClient> cosmosClientMock;
        private readonly Mock<Database> databaseMock;
        private readonly Mock<Container> containerMock;
        private readonly CosmosDbContext cosmosDbContext;
        private readonly Guid id = Guid.NewGuid();
        OrderBook orderBook = JsonSerializer.Deserialize<OrderBook>(Constants.MSG_RECEIVED_BTC);

        public CosmosDbContextTest()
        {
            this.cosmosClientMock = new Mock<CosmosClient>();
            this.databaseMock = new Mock<Database>();
            this.containerMock = new Mock<Container>();

            this.cosmosClientMock.Setup(client => client.GetDatabase(It.IsAny<string>()))
                             .Returns(databaseMock.Object);
            this.databaseMock.Setup(db => db.GetContainer(It.IsAny<string>()))
                         .Returns(containerMock.Object);

            string accountEndpoint = "";
            string authKey = "";
            string databaseId = "";
            string containerId = "";

            this.cosmosDbContext = new CosmosDbContext(accountEndpoint, authKey, databaseId, containerId);
        }

        [Fact]
        public async Task Add_ShouldAddItemToContainer()
        {
            orderBook.Id = Guid.NewGuid();
            var response = Mock.Of<ItemResponse<OrderBook>>(r => r.Resource == orderBook);

            this.containerMock.Setup(c => c.CreateItemAsync(
                It.IsAny<OrderBook>(),
                It.IsAny<PartitionKey>(),
                null,
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(response);

            var result = await this.cosmosDbContext.Add(orderBook);

            Assert.Equivalent(orderBook, result);
        }

        // TODO @gustavosg error
        //[Fact]
        //public async Task Get_ById_ShouldReturnItemWhenFound()
        //{
        //    var response = Mock.Of<ItemResponse<OrderBook>>(r => r.Resource == this.orderBook);

        //    containerMock.Setup(c => c.ReadItemAsync<OrderBook>(
        //        Id.ToString(),
        //        It.IsAny<PartitionKey>(),
        //        null,
        //        It.IsAny<CancellationToken>()
        //    )).ReturnsAsync(response);

        //    var resultGet = await cosmosDbContext.Get<OrderBook>(Id.ToString(), this.orderBook.Id.ToString());

        //    Assert.Equivalent(this.orderBook, resultGet);
        //}

        [Fact]
        public async Task Get_ById_ShouldReturnNullWhenNotFound()
        {
            this.containerMock.Setup(c => c.ReadItemAsync<OrderBook>(
                "testId",
                It.IsAny<PartitionKey>(),
                null,
                It.IsAny<CancellationToken>()
            )).ThrowsAsync(new CosmosException("Not Found", System.Net.HttpStatusCode.NotFound, 0, "", 0));

            var result = await this.cosmosDbContext.Get<OrderBook>("testId", "Id");

            Assert.Null(result);
        }

        [Fact]
        public async Task Get_ByQuery_ShouldReturnItems()
        {
            var iterator = new Mock<FeedIterator<OrderBook>>();
            iterator.SetupSequence(i => i.HasMoreResults)
                    .Returns(true)
                    .Returns(false);

            this.containerMock.Setup(c => c.GetItemQueryIterator<OrderBook>(It.IsAny<QueryDefinition>(), null, null))
                          .Returns(iterator.Object);

            var result = await this.cosmosDbContext.Get<OrderBook>("SELECT TOP 100 * FROM c");

            Assert.True(result.Any());
        }

        // TODO @gustavosg error
        //[Fact]
        //public async Task Update_ShouldReplaceItemInContainer()
        //{
        //    containerMock.Setup(c => c.ReplaceItemAsync(
        //        this.orderBook,
        //        "testId",
        //        It.IsAny<PartitionKey>(),
        //        null,
        //        It.IsAny<CancellationToken>()
        //    )).ReturnsAsync(Mock.Of<ItemResponse<OrderBook>>());

        //    await cosmosDbContext.Update(this.orderBook.Id.ToString(), this.orderBook);

        //    containerMock.Verify(c => c.ReplaceItemAsync(this.orderBook, this.orderBook.Id.ToString(), It.IsAny<PartitionKey>(), null, It.IsAny<CancellationToken>()), Times.Once);
        //}

        // TODO @gustavosg error
        //[Fact]
        //public async Task Delete_ShouldDeleteItemFromContainer()
        //{
        //    // Arrange

        //    OrderBook orderBook = (await this.cosmosDbContext.Get<OrderBook>("select top 1 * FROM c")).FirstOrDefault();

        //    //await cosmosDbContext.Add(orderBook); 

        //    // Act
        //    await cosmosDbContext.Delete<OrderBook>(orderBook);

        //    // Assert
        //    containerMock.Verify(c => c.DeleteItemAsync<OrderBook>(orderBook.Id.ToString(), It.IsAny<PartitionKey>(), null, It.IsAny<CancellationToken>()), Times.Once);
        //}
    }


}
