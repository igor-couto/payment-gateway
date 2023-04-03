namespace PaymentGatewayAPI.Configuration;

public class AppSettings
{
    public ConnectionStrings? ConnectionStrings { get; set; }
    public Jwt? Jwt { get; set; }
    public AWS? AWS { get; set; }
    public Logging? Logging { get; set; }
    public LocalStack? LocalStack { get; set; }
    public string? AllowedHosts { get; set; }
}

public class ConnectionStrings
{
    public string? DefaultConnection { get; set; }
}

public class Jwt
{
    public string? Key { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
}

public class AWS
{
    public string? AccessKeyId { get; set; }
    public string? SecretAccessKey { get; set; }
    public string? DefaultRegion { get; set; }
    public SQS? SQS { get; set; }
    public CloudWatch? CloudWatch { get; set; }
}

public class SQS
{
    public string? QueueName { get; set; }
}

public class CloudWatch
{
    public string? LogGroup { get; set; }
}

public class Logging
{
    public LogLevel? LogLevel { get; set; }
}

public class LogLevel
{
    public string? Default { get; set; }
    public string? Microsoft_AspNetCore { get; set; }
}

public class LocalStack
{
    public string? ServiceUrl { get; set; }
}
