using Moq;
using PriceListener.Infrastructure.Adapters.API.Bitstamp;
using PriceListener.Domain.Interfaces.Adapters.WebSocket;
using global::PriceListener.Domain.Entities;

namespace PriceListener.Tests.Infrastructure.Adapters.API.Bitstamp
{
    public class BitstampAdapterTest
    {
        private readonly Mock<IWebSocketAdapter> webSocketAdapterMock;
        private readonly BitstampAdapter bitstampAdapter;
        private readonly List<Cryptocurrency> cryptocurrencies = new List<Cryptocurrency>
            {
                Cryptocurrency.BTC,
                Cryptocurrency.ETH
            };

        public BitstampAdapterTest()
        {
            webSocketAdapterMock = new Mock<IWebSocketAdapter>();
            CancellationTokenSource? cancellationTokenSource = new CancellationTokenSource();
            this.bitstampAdapter = new BitstampAdapter(webSocketAdapterMock.Object, cancellationTokenSource);
        }

        [Fact]
        public async Task Subscribe_ShouldSendMessagesForAllCryptocurrencies()
        {
            // Arrange
            webSocketAdapterMock.Setup(m => m.ConnectAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            await this.bitstampAdapter.Subscribe(cryptocurrencies);

            // Assert
            webSocketAdapterMock.Verify(m => m.SendMessageAsync(It.IsAny<string>()), Times.Exactly(cryptocurrencies.Count));
        }

        [Fact]
        public async Task Unsubscribe_ShouldSendUnsubscribeMessages()
        {
            // Act
            await this.bitstampAdapter.Unsubscribe(cryptocurrencies);

            // Assert
            webSocketAdapterMock.Verify(m => m.SendMessageAsync(It.IsAny<string>()), Times.Exactly(cryptocurrencies.Count));
            webSocketAdapterMock.Verify(m => m.DisconnectAsync(), Times.Once);
        }

        [Fact]
        public async Task ReceiveMessagesAsync_ShouldCallReceiveMessage()
        {
            // Act
            await this.bitstampAdapter.ReceiveMessagesAsync();

            // Assert
            webSocketAdapterMock.Verify(m => m.ReceiveMessageAsync(), Times.Once);
        }
    }
}

