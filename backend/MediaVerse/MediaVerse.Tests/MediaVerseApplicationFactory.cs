using MediaVerse.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;
using Respawn;

namespace MediaVerse.Tests;

using Microsoft.AspNetCore.Mvc.Testing;

public class MediaVerseApplicationFactory<T> : WebApplicationFactory<T> where T : class
{
    private readonly PostgreSqlContainer _postgreSqlContainer;

    private static readonly Checkpoint Checkpoint = new()
    {
        DbAdapter = DbAdapter.Postgres,
        SchemasToInclude = ["public"]
    };

    public MediaVerseApplicationFactory()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:17.0")
            .WithBindMount(Path.Combine(Directory.GetCurrentDirectory(), "Database/init.sql"),
                "/docker-entrypoint-initdb.d/init.sql")
            .WithBindMount(Path.Combine(Directory.GetCurrentDirectory(), "Database/data"), "/data")
            .WithBindMount(Path.Combine(Directory.GetCurrentDirectory(), "Database/inserts.sql"), "/inserts")
            .WithEnvironment("POSTGRES_PASSWORD", "postgres")
            .WithPortBinding(5433, 5432)
            .Build();
    }
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        _postgreSqlContainer.StartAsync().Wait();
        
        _postgreSqlContainer.ExecAsync(["psql", "-U", "postgres", "-f", "inserts/inserts.sql"]).Wait();


        builder.ConfigureServices(services =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();

            configuration["DefaultConnection"] = _postgreSqlContainer.GetConnectionString();
        });

        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });

        var host = builder.Build();
        host.Start();
        return host;
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        await _postgreSqlContainer.ExecAsync(["psql", "-U", "postgres", "-f", "inserts/inserts.sql"]);

        Console.WriteLine(_postgreSqlContainer.GetConnectionString());
    }

    private bool _isDisposed;

    protected override void Dispose(bool disposing)
    {
        if (disposing && _isDisposed == false)
        {
            var scope = Services.CreateScope();
            _postgreSqlContainer.StopAsync().Wait();
        }

        _isDisposed = true;
    }

    public async Task DeleteAllDataAsync()
    {
        var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetService<Context>();
        await context.Database.OpenConnectionAsync();
        var dbConnection = context.Database.GetDbConnection();
        await Checkpoint.Reset(dbConnection);
        var result = await _postgreSqlContainer.ExecAsync(["psql", "-U", "postgres", "-f", "inserts"]);

        await context.Database.CloseConnectionAsync();
    }
}