using Microsoft.Extensions.DependencyInjection;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Infrastructure.Configuration;

public static class SqsConfiguration
{
    public static void AddSqs(this IServiceCollection services, bool isDevelopment, string accessKeyId, string secretAccessKey, string region, string localstackUrl)
    {
        var awsOptions = new AWSOptions
        {
            Credentials = new BasicAWSCredentials(accessKeyId, secretAccessKey),
            Region = RegionEndpoint.GetBySystemName(region),
        };

        if (isDevelopment)
        {
            awsOptions.DefaultClientConfig.ServiceURL = localstackUrl;
            awsOptions.DefaultClientConfig.UseHttp = true;
            awsOptions.DefaultClientConfig.AuthenticationRegion = region;
        }

        services.AddAWSService<IAmazonSQS>(awsOptions);

        if (isDevelopment)
        {
            var serviceProvider = services.BuildServiceProvider();
            var sqsClient = serviceProvider.GetRequiredService<IAmazonSQS>();
            var queueName = "payments.fifo";
            var createQueueRequest = new CreateQueueRequest 
            { 
                QueueName = queueName,
                Attributes = new Dictionary<string, string>
                {
                    { QueueAttributeName.FifoQueue, "true" },
                    { QueueAttributeName.ContentBasedDeduplication, "true" }
                }
            };
            sqsClient.CreateQueueAsync(createQueueRequest).GetAwaiter().GetResult();
            // TODO
            //ConfigureDeadLetterQueue(sqsClient);
        }
    }
    //public void ConfigureDeadLetterQueue(IAmazonSQS sqsClient)
    //{
    //    string mainQueueName = "payments.fifo";
    //    string deadLetterQueueName = "payments_deadletter.fifo";

    //    // Create the dead letter queue
    //    string deadLetterQueueUrl = await CreateQueue(deadLetterQueueName, sqsClient);
    //    string deadLetterQueueArn = await GetQueueArnAsync(deadLetterQueueUrl, sqsClient);

    //    // Create the main queue with a Redrive Policy that points to the dead letter queue
    //    string redrivePolicy = $"{{\"deadLetterTargetArn\":\"{deadLetterQueueArn}\",\"maxReceiveCount\":\"5\"}}";
    //    string mainQueueUrl = await CreateQueueAsync(mainQueueName, sqsClient, redrivePolicy);

    //    Console.WriteLine($"Main Queue URL: {mainQueueUrl}");
    //    Console.WriteLine($"Dead Letter Queue URL: {deadLetterQueueUrl}");
    //}
}
