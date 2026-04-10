using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Serilog;
using YardView.TaskManager.Server.Data;
using YardView.TaskManager.Server.Endpoints;
using YardView.TaskManager.Server.Extensions;
using YardView.TaskManager.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.BuildConnectionString(false));
});

builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo 
    { 
        Title = "Task Manager API", 
        Version = "v1" 
    });
});

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
    
    app.MapOpenApi();


}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    // UI will be available at /swagger
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Manager API V1");
    options.RoutePrefix = "swagger";
    // optional: collapse schema models by default
    options.DefaultModelsExpandDepth(-1);
});

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapTaskEndpoints();

app.MapFallbackToFile("/index.html");

app.Run();
