using MediaVerse.Infrastructure.Database;
using MediaVerse.Infrastructure.Migrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace MediaVerse.Host.Extensions;

public static class HostExtensions
{
    public static WebApplication UseReactServing(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            var path = (string)context.Request.Path;
            if (!path.StartsWith("/api") && !Path.HasExtension(path))
            {
                context.Request.Path = (PathString)"/index.html";
            }

            await next();
        });
        app.UseStaticFiles();

        return app;
    }
    
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