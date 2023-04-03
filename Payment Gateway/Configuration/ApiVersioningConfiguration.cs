using Microsoft.AspNetCore.Mvc;

namespace PaymentGatewayAPI.Configuration;

public static class ApiVersioningConfiguration
{
    public static void AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new ApiVersion(int.Parse(CurrentApiVersion.Major), 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
        });
    }
}

public static class CurrentApiVersion
{
    public const string Major = "1";
}
