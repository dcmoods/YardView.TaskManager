using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Serilog;
using YardView.TaskManager.Server.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(BuildConnectionString(builder.Configuration, false));
});

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

string BuildConnectionString(IConfiguration configuration, bool readOnly)
{
    var configuredConnectionString = configuration.GetConnectionString("TasksDatabase")
        ?? throw new InvalidOperationException("Connection string 'CmsDatabase' is not configured.");

    var builder = new SqliteConnectionStringBuilder(configuredConnectionString)
    {
        Cache = SqliteCacheMode.Shared
    };

    var dataSource = builder.DataSource;
    if (string.IsNullOrWhiteSpace(dataSource))
    {
        throw new InvalidOperationException("Connection string 'CmsDatabase' must include a Data Source.");
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