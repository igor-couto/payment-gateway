using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Infrastructure.Persistence;

namespace PaymentGatewayAPI.Configuration;

public static class HealthCheckConfiguration
{
    public static void AddHealthCheck(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddCheck<PostgresHealthChecks>("database", HealthStatus.Unhealthy, timeout: TimeSpan.FromSeconds(4));
    }

    public static void UseHealthCheckConfiguration(this WebApplication app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions { Predicate = _ => true, ResponseWriter = WriteResponse });
    }

    private static Task WriteResponse(HttpContext context, HealthReport healthReport)
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        var options = new JsonWriterOptions { Indented = true };

        using var memoryStream = new MemoryStream();
        using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
        {
            jsonWriter.WriteStartObject();
            jsonWriter.WriteString("status", healthReport.Status.ToString());
            jsonWriter.WriteStartObject("results");

            foreach (var healthReportEntry in healthReport.Entries)
            {
                jsonWriter.WriteStartObject(healthReportEntry.Key);
                jsonWriter.WriteString("status", healthReportEntry.Value.Status.ToString());
                jsonWriter.WriteString("description", healthReportEntry.Value.Description);

                foreach (var item in healthReportEntry.Value.Data)
                {
                    jsonWriter.WritePropertyName(item.Key);

                    JsonSerializer.Serialize(jsonWriter, item.Value,
                        item.Value?.GetType() ?? typeof(object));
                }

                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndObject();
            }

            jsonWriter.WriteEndObject();
            jsonWriter.WriteEndObject();
        }
        return context.Response.WriteAsync(Encoding.UTF8.GetString(memoryStream.ToArray()));
    }
}
