using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Domain.Messages;
using PaymentGatewayAPI.Services.Interfaces;

namespace PaymentGatewayAPI.Services;

public class ExecutePaymentMessagePublisherService : IExecutePaymentMessagePublisherService
{
    private readonly IAmazonSQS _sqsClient;
    private readonly string _queueName;

    public ExecutePaymentMessagePublisherService(IAmazonSQS sqsClient, IConfiguration configuration)
    {
        _sqsClient = sqsClient;
        _queueName = configuration["AWS:SQS:QueueName"]!;
    }

    public async Task Publish(ExecutePaymentMessage executePaymentMessage, CancellationToken cancellationToken)
    {
        var queueUrlResponse = await _sqsClient.GetQueueUrlAsync(_queueName, cancellationToken);

        var request = new SendMessageRequest
        {
            QueueUrl = queueUrlResponse.QueueUrl,
            MessageBody = JsonSerializer.Serialize(executePaymentMessage),
            MessageGroupId = Guid.NewGuid().ToString(),
            MessageDeduplicationId = executePaymentMessage.Id.ToString(),
        };

        await _sqsClient.SendMessageAsync(request, cancellationToken);
    }
}
