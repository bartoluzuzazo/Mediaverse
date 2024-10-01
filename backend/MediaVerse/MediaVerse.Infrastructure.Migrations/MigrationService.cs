using System.Reflection;
using DbUp;
using Serilog;

namespace MediaVerse.Infrastructure.Migrations;

public class MigrationService(string connectionString)
{
    public void Migrate()
    {
        var upgradeEngine = DeployChanges.To.PostgresqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(typeof(MigrationService).GetTypeInfo().Assembly)
            .LogTo(new UpgradeLogger())
            .WithVariablesDisabled()
            .Build();

        if (upgradeEngine is null)
        {
            Log.Error("MigrationService has not been deployed");
            return;
        }
        
        var result = upgradeEngine.PerformUpgrade();

        if (!result.Successful)
        {
            Log.Error("Migration failed:\n {0}", result.Error.ToString());
        }
        else
        {
            Log.Information("Migration completed successfully");
        }
    }
}