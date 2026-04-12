using FluentValidation;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Serilog;
using YardView.TaskManager.Server.Data;
using YardView.TaskManager.Server.Data.Extensions;
using YardView.TaskManager.Server.Endpoints;
using YardView.TaskManager.Server.Services;
using YardView.TaskManager.Server.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(new string[] { "https://localhost:64860", "http://localhost:4200" })
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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

builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskRequestValidator>();

builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddEndpointsApiExplorer();
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
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Manager API V1");
    options.RoutePrefix = "swagger";
    options.DefaultModelsExpandDepth(-1);
});

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("Frontend");

app.MapTaskEndpoints();
app.MapTaskStatusEndpoints();

app.MapFallbackToFile("/index.html");

app.Run();
