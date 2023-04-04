using Amazon.SQS;
using AutoBogus;
using Domain.Messages;
using PaymentGatewayAPI.Services;
using NUnit.Framework;
using NSubstitute;
using Amazon.SQS.Model;
using System.Text.Json;

namespace PaymentGatewayUnitAPI.Services;

class ExecutePaymentMessagePublisherServiceTests
{
    private ExecutePaymentMessagePublisherService _messagePublisherService;
    private IAmazonSQS _sqsClientMock;
    private CancellationToken _cancellationToken;
    private const string QueueUrl = "https://example.com/sqs/payments.fifo";
    private readonly string QueueName = "payments.fifo";


    [SetUp]
    public void SetUp()
    {
        _cancellationToken = new CancellationTokenSource().Token;

        _sqsClientMock = Substitute.For<IAmazonSQS>();

        _sqsClientMock.GetQueueUrlAsync(QueueName, _cancellationToken)
            .Returns(Task.FromResult(new GetQueueUrlResponse { QueueUrl = QueueUrl }));

        _messagePublisherService = new ExecutePaymentMessagePublisherService(_sqsClientMock);
    }

    [Test]
    public async Task Publish_ShouldSendMessageToSQSQueue()
    {
        // Arrange
        var executePaymentMessage = AutoFaker.Generate<ExecutePaymentMessage>();

        var request = new SendMessageRequest
        {
            QueueUrl = QueueUrl,
            MessageBody = JsonSerializer.Serialize(executePaymentMessage),
            MessageGroupId = Guid.NewGuid().ToString(),
            MessageDeduplicationId = executePaymentMessage.Id.ToString(),
        };

        // Act
        await _messagePublisherService.Publish(executePaymentMessage, _cancellationToken);

        // Assert
        await _sqsClientMock.Received(1).SendMessageAsync(Arg.Any<SendMessageRequest>(), _cancellationToken );
    }
}
