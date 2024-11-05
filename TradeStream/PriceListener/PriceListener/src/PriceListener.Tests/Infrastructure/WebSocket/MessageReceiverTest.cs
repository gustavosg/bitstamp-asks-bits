using Moq;
using PriceListener.Domain.Interfaces.Adapters.WebSocket;
using PriceListener.Infrastructure.Adapters.WebSocket;
using System;
using System.Net.WebSockets;
using System.Text;

namespace PriceListener.Tests.Infrastructure.WebSocket
{
    public class MessageReceiverTest
    {
        private readonly Mock<IWebSocketConnector> connectorMock;
        private readonly Mock<IClientWebSocketWrapper> clientWebSocketMock;
        private readonly MessageReceiver receiverMock;
        private readonly CancellationTokenSource cancellationTokenSource;

        public MessageReceiverTest()
        {
            this.connectorMock = new Mock<IWebSocketConnector>();
            this.clientWebSocketMock = new Mock<IClientWebSocketWrapper>();
            this.cancellationTokenSource = new CancellationTokenSource();

            this.connectorMock.Setup(connector => connector.GetClientWebSocket()).Returns(clientWebSocketMock.Object);

            this.receiverMock = new MessageReceiver(this.connectorMock.Object, this.cancellationTokenSource);
        }

        [Fact]
        public async Task ReceiveDataAsync_ShouldReceiveAndInvokeOnMessageReceived()
        {
            // Arrange
            var receivedMessage = "Receiving Test";
            var buffer = Encoding.UTF8.GetBytes(receivedMessage);

            this.clientWebSocketMock
                .Setup(ws => ws.State)
                .Returns(WebSocketState.Open);
            this.clientWebSocketMock
                .Setup(ws => ws.ReceiveAsync(It.IsAny<ArraySegment<byte>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new WebSocketReceiveResult(buffer.Length, WebSocketMessageType.Text, true))
                .Callback((ArraySegment<byte> arraySegment, CancellationToken token) =>
                {
                    Buffer.BlockCopy(buffer, 0, arraySegment.Array, 0, buffer.Length);
                    
                    this.clientWebSocketMock.Setup(ws => ws.ReceiveAsync(It.IsAny<ArraySegment<byte>>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new WebSocketReceiveResult(0, WebSocketMessageType.Close, true));
                });

            string invokedMessage = null;
            this.receiverMock.OnMessageReceived += message => invokedMessage = message;

            // Act
            var receiveTask = this.receiverMock.ReceiveDataAsync();
            await Task.Delay(100);
            this.cancellationTokenSource.Cancel();

            // Assert
            await receiveTask; 
            Assert.Equal(receivedMessage, invokedMessage);
        }

        [Fact]
        public async Task ReceiveDataAsync_ShouldCloseWebSocketOnCancellationRequest()
        {
            // Arrange
            var receivedMessage = "Receiving Test";
            var buffer = Encoding.UTF8.GetBytes(receivedMessage);

            this.clientWebSocketMock
               .Setup(ws => ws.State)
               .Returns(WebSocketState.Open);
            this.clientWebSocketMock
                .Setup(ws => ws.ReceiveAsync(It.IsAny<ArraySegment<byte>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new WebSocketReceiveResult(buffer.Length, WebSocketMessageType.Text, true))
                .Callback((ArraySegment<byte> arraySegment, CancellationToken token) =>
                {
                    Buffer.BlockCopy(buffer, 0, arraySegment.Array, 0, buffer.Length);

                    this.cancellationTokenSource.Cancel();

                    this.clientWebSocketMock.Setup(ws => ws.ReceiveAsync(It.IsAny<ArraySegment<byte>>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new WebSocketReceiveResult(0, WebSocketMessageType.Close, true));
                });

            // Act
            await this.receiverMock.ReceiveDataAsync();

            // Assert
            this.clientWebSocketMock.Verify(ws => ws.CloseAsync(WebSocketCloseStatus.NormalClosure, It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ReceiveDataAsync_ShouldNotInvokeOnMessageReceived_WhenMessageTypeIsClose()
        {
            // Arrange
            this.clientWebSocketMock
                .Setup(ws => ws.ReceiveAsync(It.IsAny<ArraySegment<byte>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new WebSocketReceiveResult(0, WebSocketMessageType.Close, true));

            bool wasInvoked = false;
            this.receiverMock.OnMessageReceived += _ => wasInvoked = true;

            // Act
            await this.receiverMock.ReceiveDataAsync();

            // Assert
            Assert.False(wasInvoked);
        }
    }
}
