using System.Net.WebSockets;
using Moq;
using PriceListener.Domain.Interfaces.Adapters.WebSocket;
using PriceListener.Infrastructure.Adapters.WebSocket;

namespace PriceListener.Tests.Infrastructure.WebSocket
{
    public class WebSocketConnectorTest
    {
        private readonly Mock<IClientWebSocketWrapper> clientWebSocketMock;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly WebSocketConnector webSocketConnector;

        public WebSocketConnectorTest()
        {
            this.clientWebSocketMock = new Mock<IClientWebSocketWrapper>();
            this.cancellationTokenSource = new CancellationTokenSource();
            this.webSocketConnector = new WebSocketConnector(
                this.clientWebSocketMock.Object,
                this.cancellationTokenSource
            );
        }

        [Fact]
        public async Task ConnectAsync_ShouldConnect_WhenWebSocketIsNotOpen()
        {
            var uri = "wss://test.com";

            this.clientWebSocketMock.Setup(ws => ws.State).Returns(WebSocketState.Closed);

            await this.webSocketConnector.ConnectAsync(uri);

            this.clientWebSocketMock.Verify(ws => ws.ConnectAsync(new Uri(uri), this.cancellationTokenSource.Token), Times.Once);
        }

        [Fact]
        public async Task ConnectAsync_ShouldNotConnect_WhenWebSocketIsAlreadyOpen()
        {
            var uri = "wss://test.com";

            this.clientWebSocketMock.Setup(ws => ws.State).Returns(WebSocketState.Open);

            await this.webSocketConnector.ConnectAsync(uri);

            this.clientWebSocketMock.Verify(ws => ws.ConnectAsync(It.IsAny<Uri>(), this.cancellationTokenSource.Token), Times.Never);
        }

        [Fact]
        public async Task DisconnectAsync_ShouldDisconnect_WhenWebSocketIsOpen()
        {
            this.clientWebSocketMock.Setup(ws => ws.State).Returns(WebSocketState.Open);

            await this.webSocketConnector.DisconnectAsync();

            this.clientWebSocketMock.Verify(ws => ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnect", this.cancellationTokenSource.Token), Times.Once);
        }

        [Fact]
        public async Task DisconnectAsync_ShouldNotDisconnect_WhenWebSocketIsNotOpen()
        {
            this.clientWebSocketMock.Setup(ws => ws.State).Returns(WebSocketState.Closed);

            await this.webSocketConnector.DisconnectAsync();

            this.clientWebSocketMock.Verify(ws => ws.CloseAsync(It.IsAny<WebSocketCloseStatus>(), It.IsAny<string>(), this.cancellationTokenSource.Token), Times.Never);
        }

        [Fact]
        public void GetClientWebSocket_ShouldReturnClientWebSocketInstance()
        {
            var result = this.webSocketConnector.GetClientWebSocket();

            Assert.Equal(this.clientWebSocketMock.Object, result);
        }
    }
}
