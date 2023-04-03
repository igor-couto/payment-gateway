using System.Net;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Domain.Messages;
using Infrastructure.Persistence;
using PaymentExecutor.Exceptions;
using PaymentExecutor.Services;

namespace PaymentExecutor;

public class Consumer : BackgroundService
{
    private readonly ILogger<Consumer> _logger;
    private readonly HttpClient _acquiringBankHttpClient;
    private readonly IAmazonSQS _sqsClient;
    private readonly List<string> _messageAttributeNames = new() { "All" };
    private readonly List<string> _attributeNames = new() { "All" };
    //private readonly IExecutePaymentService _executePaymentService;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private string? _currentReceiptHandle;


    public Consumer(ILogger<Consumer> logger, IAmazonSQS sqsClient, IHttpClientFactory httpClientFactory, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _sqsClient = sqsClient;
        _acquiringBankHttpClient = httpClientFactory.CreateClient("AcquiringBankSimulator");
        //_executePaymentService = new ExecutePaymentService(logger, databaseContext, _acquiringBankHttpClient);
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var queueUrlResponse = await _sqsClient.GetQueueUrlAsync("payments.fifo", cancellationToken);
        var receiveRequest = new ReceiveMessageRequest
        {
            QueueUrl = queueUrlResponse.QueueUrl,
            MessageAttributeNames = _messageAttributeNames,
            AttributeNames = _attributeNames,
            MaxNumberOfMessages = 5,
            WaitTimeSeconds = 10
        };

        using var scope = _serviceScopeFactory.CreateScope();
        var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        var executePaymentService = new ExecutePaymentService(_logger, databaseContext, _acquiringBankHttpClient);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var messageResponse = await _sqsClient.ReceiveMessageAsync(receiveRequest, cancellationToken);

                if (messageResponse.HttpStatusCode != HttpStatusCode.OK)
                    continue;

                foreach (var message in messageResponse.Messages)
                {
                    _currentReceiptHandle = message.ReceiptHandle;

                    var executePaymentMessage = JsonSerializer.Deserialize<ExecutePaymentMessage>(message.Body);
                    await executePaymentService.Execute(executePaymentMessage!, cancellationToken);

                    await _sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, cancellationToken);
                }
            }
            catch (FailedToExecutePaymentException exception)
            {
                _logger.LogError("Error processing message: {Message}", exception.Message);
                await _sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, _currentReceiptHandle, cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError("Error processing message: {Message}", exception.Message);
            }
        }
    }
}