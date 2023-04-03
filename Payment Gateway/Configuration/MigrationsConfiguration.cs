using FluentMigrator.Runner;
using System.Reflection;

namespace PaymentGatewayAPI.Configuration;

public static class MigrationsConfiguration
{
    public static void AddFluentMigrator(this IServiceCollection services, string connectionString)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(
                runnerBuilder =>
                {
                    runnerBuilder
                        .AddPostgres()
                        .WithGlobalConnectionString(connectionString)
                        //.ScanIn(Assembly.GetExecutingAssembly()).For.Migrations()
                        .ScanIn(Assembly.GetAssembly(typeof(Infrastructure.Persistence.DatabaseContext))).For.Migrations()
                        .For.All();
                })
            .AddLogging(lb => lb.AddFluentMigratorConsole());
    }

    public static void UseFluentMigratorConfiguration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var migrator = scope.ServiceProvider.GetService<IMigrationRunner>();
        migrator!.MigrateUp();
    }
}