using DbUp.Engine.Output;
using Serilog;

namespace MediaVerse.Infrastructure.Migrations;

public class UpgradeLogger : IUpgradeLog
{
    public void LogError(string format, params object[] args) => Log.Error(format, args);

    public void LogError(Exception ex, string format, params object[] args) => Log.Error(ex, format, args);

    public void LogWarning(string format, params object[] args) => Log.Warning(format, args);

    public void LogTrace(string format, params object[] args) => Log.Information(format, args);

    public void LogDebug(string format, params object[] args) => Log.Debug(format, args);

    public void LogInformation(string format, params object[] args) => Log.Information(format, args);
}