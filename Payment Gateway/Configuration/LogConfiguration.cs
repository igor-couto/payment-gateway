using Amazon;
using Amazon.CloudWatchLogs;
using Amazon.Runtime;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.AwsCloudWatch;

namespace PaymentGatewayAPI.Configuration;

public static class LogConfiguration
{
    public static void AddLogs(this ConfigureHostBuilder host, string accessKeyId, string secretAccessKey, string region, string logGroup)
    {
        host.UseSerilog((hostingContext, loggerConfiguration) =>
        {
            var isDevelopment = hostingContext.HostingEnvironment.IsDevelopment();

            if (isDevelopment)
            {
                loggerConfiguration
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .WriteTo.Console();
            }
            else
            {
                var awsCredentials = new BasicAWSCredentials(accessKeyId, secretAccessKey);
                var cloudWatchLogsClient = new AmazonCloudWatchLogsClient(awsCredentials, RegionEndpoint.GetBySystemName(region));
                var logStreamNameProvider = new DefaultLogStreamProvider();

                var cloudWatchSinkOptions = new CloudWatchSinkOptions
                {
                    LogGroupName = logGroup,
                    LogStreamNameProvider = logStreamNameProvider,
                    TextFormatter = new JsonFormatter(),
                    MinimumLogEventLevel = LogEventLevel.Information,
                    BatchSizeLimit = 100,
                    QueueSizeLimit = 10000,
                    Period = TimeSpan.FromSeconds(10),
                    CreateLogGroup = true
                };

                loggerConfiguration
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .WriteTo.AmazonCloudWatch(cloudWatchSinkOptions, cloudWatchLogsClient);
            }
        });
    }
}
