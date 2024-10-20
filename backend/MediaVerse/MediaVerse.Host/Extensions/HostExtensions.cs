using MediaVerse.Infrastructure.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace MediaVerse.Host.Extensions;

public static class HostExtensions
{
    public static void ExecuteDbMigrations(this IHost host)
    {
        var configuration = host.Services.GetService<IConfiguration>() ?? throw new Exception("Configuration is null");


        var connectionString = configuration["DefaultConnection"] ??
                               throw new Exception("Default connection string");

        var migrationService = new MigrationService(connectionString);
        migrationService.Migrate();
    }

    public static void AddLogging(this IHostApplicationBuilder hostBuilder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        hostBuilder.Services.AddSerilog();
    }
}