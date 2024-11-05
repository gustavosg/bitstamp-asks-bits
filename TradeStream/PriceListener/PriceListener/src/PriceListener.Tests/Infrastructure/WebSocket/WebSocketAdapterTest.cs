using Moq;
using PriceListener.Domain.Interfaces.Adapters.WebSocket;
using PriceListener.Infrastructure.Adapters.WebSocket;

namespace PriceListener.Tests.Infrastructure.WebSocket
{
    public class WebSocketAdapterTest
    {
        private readonly Mock<IWebSocketConnector> connectorMock;
        private readonly Mock<IMessageReceiver> receiverMock;
        private readonly Mock<IMessageSender> senderMock;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly WebSocketAdapter webSocketAdapter;

        public WebSocketAdapterTest()
        {
            this.connectorMock = new Mock<IWebSocketConnector>();
            this.receiverMock = new Mock<IMessageReceiver>();
            this.senderMock = new Mock<IMessageSender>();
            this.cancellationTokenSource = new CancellationTokenSource();

            this.webSocketAdapter = new WebSocketAdapter(
                this.connectorMock.Object,
                this.receiverMock.Object,
                this.senderMock.Object,
                this.cancellationTokenSource
            );
        }

        [Fact]
        public async Task ConnectAsync_ShouldCallConnectorConnectAsync()
        {
            var uri = "wss://test.com";

            await this.webSocketAdapter.ConnectAsync(uri);

            this.connectorMock.Verify(connector => connector.ConnectAsync(uri), Times.Once);
        }

        [Fact]
        public async Task ReceiveMessageAsync_ShouldCallReceiverReceiveDataAsync()
        {
            await this.webSocketAdapter.ReceiveMessageAsync();

            this.receiverMock.Verify(receiver => receiver.ReceiveDataAsync(), Times.Once);
        }

        [Fact]
        public async Task SendMessageAsync_ShouldCallSenderSendMessageAsync()
        {
            var message = "Hello WebSocket";

            await this.webSocketAdapter.SendMessageAsync(message);

            this.senderMock.Verify(sender => sender.SendMessageAsync(message), Times.Once);
        }

        [Fact]
        public async Task DisconnectAsync_ShouldCallConnectorDisconnectAsync()
        {
            await this.webSocketAdapter.DisconnectAsync();

            this.connectorMock.Verify(connector => connector.DisconnectAsync(), Times.Once);
        }

        [Fact]
        public void OnMessageReceived_ShouldInvokeEvent_WhenReceiverOnMessageReceivedIsRaised()
        {
            string testMessage = "Test message";
            string receivedMessage = null;
            this.webSocketAdapter.OnMessageReceived += message => receivedMessage = message;

            this.receiverMock.Raise(receiver => receiver.OnMessageReceived += null, testMessage);

            Assert.Equal(testMessage, receivedMessage);
        }
    }
}
