using YardView.TaskManager.Api.Mapping;

namespace YardView.TaskManager.Server.Endpoints;

public static class TaskStatusEndpoints 
{
    public static IEndpointRouteBuilder MapTaskStatusEndpoints(this IEndpointRouteBuilder app)
    {

        var group = app.MapGroup("/task-statuses")
                        .WithTags("Task Statuses");

        group.MapGet("/", () =>
        {
            var statuses = Enum.GetValues<Models.TaskStatus>()
                        .Select(x => TaskStatusMapper.ToApiValue(x))
                        .ToList();

            return Results.Ok(statuses);
        })
        .Produces<IEnumerable<string>>(StatusCodes.Status200OK);


        return app;
    }
}
