using System.Text;
using System.Net.WebSockets;
using Moq;
using PriceListener.Infrastructure.Adapters.WebSocket;
using PriceListener.Domain.Interfaces.Adapters.WebSocket;

namespace PriceListener.Tests.Infrastructure.WebSocket
{

    public class MessageSenderTest
    {
        private readonly Mock<IClientWebSocketWrapper> clientWebSocketMock;
        private readonly Mock<IWebSocketConnector> webSocketConnectorMock;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly MessageSender _messageSender;

        public MessageSenderTest()
        {
            this.clientWebSocketMock = new Mock<IClientWebSocketWrapper>();
            this.webSocketConnectorMock = new Mock<IWebSocketConnector>();
            this.cancellationTokenSource = new CancellationTokenSource();

            this.webSocketConnectorMock
                .Setup(connector => connector.GetClientWebSocket())
                .Returns(this.clientWebSocketMock.Object);

            _messageSender = new MessageSender(this.webSocketConnectorMock.Object, this.cancellationTokenSource);
        }

        [Fact]
        public async Task SendMessageAsync_ShouldSendCorrectMessage()
        {
            // arrange
            var message = "Test message";
            var expectedBuffer = Encoding.UTF8.GetBytes(message);

            this.clientWebSocketMock
                .Setup(ws => ws.SendAsync(It.IsAny<ArraySegment<byte>>(), WebSocketMessageType.Text, true, this.cancellationTokenSource.Token))
                .Returns(Task.CompletedTask)
                .Callback((ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken token) =>
                {
                    Assert.Equal(expectedBuffer, buffer.ToArray());
                    Assert.Equal(WebSocketMessageType.Text, messageType);
                    Assert.True(endOfMessage);
                    Assert.Equal(this.cancellationTokenSource.Token, token);
                });

            // act
            await _messageSender.SendMessageAsync(message);

            // assert
            this.clientWebSocketMock.Verify(ws => ws.SendAsync(It.IsAny<ArraySegment<byte>>(), WebSocketMessageType.Text, true, this.cancellationTokenSource.Token), Times.Once);
        }

        [Fact]
        public async Task SendMessageAsync_ShouldThrowArgumentNullException_WhenMessageIsNull()
        {
            // arrange
            string nullMessage = null;

            // act / assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _messageSender.SendMessageAsync(nullMessage));
        }

        [Fact]
        public async Task SendMessageAsync_ShouldSendMultipleMessagesSequentially()
        {
            // arrange
            var messages = new List<string> { "Message1", "Message2", "Message3" };

            foreach (var message in messages)
            {
                var expectedBuffer = Encoding.UTF8.GetBytes(message);

                this.clientWebSocketMock
                    .Setup(ws => ws.SendAsync(It.IsAny<ArraySegment<byte>>(), WebSocketMessageType.Text, true, this.cancellationTokenSource.Token))
                    .Returns(Task.CompletedTask)
                    .Callback((ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken token) =>
                    {
                        Assert.Equal(expectedBuffer, buffer.ToArray());
                        Assert.Equal(WebSocketMessageType.Text, messageType);
                        Assert.True(endOfMessage);
                        Assert.Equal(this.cancellationTokenSource.Token, token);
                    });
            }

            // act
            messages.ForEach(async message => await _messageSender.SendMessageAsync(message));

            // assert
            this.clientWebSocketMock.Verify(ws => ws.SendAsync(It.IsAny<ArraySegment<byte>>(), WebSocketMessageType.Text, true, this.cancellationTokenSource.Token), Times.Exactly(messages.Count));
        }
    }
}
