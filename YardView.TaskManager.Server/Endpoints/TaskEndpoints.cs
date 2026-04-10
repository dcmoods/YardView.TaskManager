using FluentValidation;
using System.ComponentModel.DataAnnotations;
using YardView.TaskManager.Server.Contracts.Tasks;
using YardView.TaskManager.Server.Services;

namespace YardView.TaskManager.Server.Endpoints;

public static class TaskEndpoints
{
    public static IEndpointRouteBuilder MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/tasks")
                        .WithTags("Tasks");

        group.MapGet("/", async (string? status, ITaskService taskService, CancellationToken ct) =>
        {
            var tasks = await taskService.GetTasksAsync(status, ct);
            return Results.Ok(tasks);
        })
        .Produces<IEnumerable<TaskResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", async (int id, ITaskService taskService, CancellationToken ct) =>
        {
            if (id <= 0)
            {
                return Results.BadRequest("Invalid task ID.");
            }

            var task = await taskService.GetByIdAsync(id, ct);
            return task is not null ? Results.Ok(task) : Results.NotFound();
        })
        .Produces<TaskResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (
            CreateTaskRequest request,
            ITaskService taskService,
            IValidator<CreateTaskRequest> Validator,
            CancellationToken ct) =>
        {
            var validationResult = await Validator.ValidateAsync(request, ct);
            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());


            var createdTask = await taskService.CreateAsync(request, ct);
            return Results.Created($"/tasks/{createdTask.Id}", createdTask);
        })
        .Produces<TaskResponse>(StatusCodes.Status201Created)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", async (
            int id, 
            UpdateTaskRequest request, 
            ITaskService taskService, 
            IValidator<UpdateTaskRequest> validator,
            CancellationToken ct) =>
        {
            if (id <= 0)
                return Results.BadRequest("Invalid task ID.");

            var validationResult = await validator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
                return Results.ValidationProblem(validationResult.ToDictionary());

            var updatedTask = await taskService.UpdateAsync(id, request, ct);
            return updatedTask is not null ? Results.Ok(updatedTask) : Results.NotFound();
        })
        .Produces<TaskResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);


        group.MapDelete("/{id:int}", async (int id, ITaskService taskService, CancellationToken ct) =>
        {
            var deleted = await taskService.DeleteAsync(id, ct);
            return deleted ? Results.NoContent() : Results.NotFound();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }

}
