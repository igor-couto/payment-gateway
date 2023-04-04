using Microsoft.Extensions.DependencyInjection;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configuration;

public static class SqsConfiguration
{
    public static void AddSqs(this IServiceCollection services, bool isDevelopment, IConfiguration configuration)
    {
        var accessKeyId = configuration["AWS:AccessKeyId"]!;
        var secretAccessKey = configuration["AWS:SecretAccessKey"]!;
        var region = configuration["AWS:DefaultRegion"]!;
        var localstackUrl = configuration["LocalStack:ServiceUrl"]!;
        var queueName = configuration["AWS:SQS:QueueName"]!;

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
            // TODO: Create Dead Letter Queue using localstack
            //ConfigureDeadLetterQueue(sqsClient);
        }
    }
}
