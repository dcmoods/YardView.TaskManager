using Microsoft.Data.Sqlite;
using YardView.TaskManager.Server.Data;

namespace YardView.TaskManager.Server.Extensions;

public static class DbExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await DbInitializer.InitializeAsync(dbContext);
    }


    public static string BuildConnectionString(this IConfiguration configuration, bool readOnly)
    {
        var configuredConnectionString = configuration.GetConnectionString("TasksDatabase")
            ?? throw new InvalidOperationException("Connection string 'TasksDatabase' is not configured.");

        var builder = new SqliteConnectionStringBuilder(configuredConnectionString)
        {
            Cache = SqliteCacheMode.Shared
        };

        var dataSource = builder.DataSource;
        if (string.IsNullOrWhiteSpace(dataSource))
        {
            throw new InvalidOperationException("Connection string 'TasksDatabase' must include a Data Source.");
        }

        if (!Path.IsPathRooted(dataSource))
        {
            dataSource = Path.GetFullPath(dataSource, AppContext.BaseDirectory);
        }

        var directory = Path.GetDirectoryName(dataSource);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        builder.DataSource = dataSource;
        builder.Mode = readOnly ? SqliteOpenMode.ReadOnly : SqliteOpenMode.ReadWriteCreate;

        return builder.ToString();
    }
}
