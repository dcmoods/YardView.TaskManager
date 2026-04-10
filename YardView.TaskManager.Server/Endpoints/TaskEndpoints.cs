using YardView.TaskManager.Server.Contracts.Tasks;
using YardView.TaskManager.Server.Services;

namespace YardView.TaskManager.Server.Endpoints;

public static class TaskEndpoints
{
    public static IEndpointRouteBuilder MapTaskEndpoints(this IEndpointRouteBuilder app)
    {

        var group = app.MapGroup("/tasks");

        group.MapGet("/", async (string? status, ITaskService taskService, CancellationToken ct) =>
        {
            var tasks = await taskService.GetTasksAsync(status, ct);
            return Results.Ok(tasks);
        });

        group.MapGet("/{id:int}", async (int id, ITaskService taskService, CancellationToken ct) =>
        {
            if (id <= 0)
            {
                return Results.BadRequest("Invalid task ID.");
            }

            var task = await taskService.GetByIdAsync(id, ct);
            return task is not null ? Results.Ok(task) : Results.NotFound();
        });

        group.MapPost("/", async (CreateTaskRequest request, ITaskService taskService, CancellationToken ct) =>
        {
            var createdTask = await taskService.CreateAsync(request, ct);
            return Results.Created($"/tasks/{createdTask.Id}", createdTask);
        });

        group.MapPut("/{id:int}", async (int id, UpdateTaskRequest request, ITaskService taskService, CancellationToken ct) =>
        {
            if (id <= 0)
            {
                return Results.BadRequest("Invalid task ID.");
            }

            var updatedTask = await taskService.UpdateAsync(id, request, ct);
            return updatedTask is not null ? Results.Ok(updatedTask) : Results.NotFound();
        });

        group.MapDelete("/{id:int}", async (int id, ITaskService taskService, CancellationToken ct) =>
        {
            var deleted = await taskService.DeleteAsync(id, ct);
            return deleted ? Results.NoContent() : Results.NotFound();
        });

        return app;
    }

}
