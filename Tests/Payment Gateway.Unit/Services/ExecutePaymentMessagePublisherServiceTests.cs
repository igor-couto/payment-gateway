using Amazon.SQS.Model;
using Amazon.SQS;
using AutoBogus;
using Domain.Messages;
using Moq;
using PaymentGatewayAPI.Services;

namespace PaymentGatewayUnitAPI.Services;

class ExecutePaymentMessagePublisherServiceTests
{
    private ExecutePaymentMessagePublisherService _messagePublisherService;
    private Mock<IAmazonSQS> _sqsClientMock;
    private const string QueueUrl = "https://example.com/sqs/payments.fifo";

    [SetUp]
    public void SetUp()
    {
        _sqsClientMock = new Mock<IAmazonSQS>();
        _sqsClientMock.Setup(client => client.GetQueueUrlAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetQueueUrlResponse { QueueUrl = QueueUrl });

        _messagePublisherService = new ExecutePaymentMessagePublisherService(_sqsClientMock.Object);
    }

    [Test]
    public async Task Publish_ShouldSendMessageToSQSQueue()
    {
        // Arrange
        var executePaymentMessage = AutoFaker.Generate<ExecutePaymentMessage>();

        // Act
        await _messagePublisherService.Publish(executePaymentMessage, CancellationToken.None);

        // Assert
        _sqsClientMock.Verify(client => client.GetQueueUrlAsync("payments.fifo", It.IsAny<CancellationToken>()), Times.Once);
    }

    [TearDown]
    public void TearDown()
    {
        _sqsClientMock.Reset();
    }
}
