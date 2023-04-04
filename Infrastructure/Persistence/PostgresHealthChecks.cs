using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Infrastructure.Persistence;

public class PostgresHealthChecks : IHealthCheck
{
    private readonly string _connectionString;
    private const string _healthQuery = "SELECT 1;";
    private readonly ILogger<PostgresHealthChecks> _logger;

    public PostgresHealthChecks(IConfiguration configuration, ILogger<PostgresHealthChecks> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = connection.CreateCommand();
            command.CommandText = _healthQuery;

            await command.ExecuteScalarAsync(cancellationToken);

            return new HealthCheckResult(HealthStatus.Healthy, "PostgreSQL database is working properly.");
        }
        catch (Exception exception)
        {
            const string errorMessage = "Failed to connect to PostgreSQL database";

            _logger.LogError(exception, errorMessage);

            return new HealthCheckResult(context.Registration.FailureStatus, errorMessage);
        }
    }
}
