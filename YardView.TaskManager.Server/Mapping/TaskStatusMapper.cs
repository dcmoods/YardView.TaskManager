using YardView.TaskManager.Server.Models;
using TaskStatus = YardView.TaskManager.Server.Models.TaskStatus;

namespace YardView.TaskManager.Api.Mapping;
public static class TaskStatusMapper
{
    public static TaskStatus ToEnum(string status) => status.ToLowerInvariant() switch
    {
        "todo" => TaskStatus.Todo,
        "in_progress" => TaskStatus.InProgress,
        "done" => TaskStatus.Done,
        _ => throw new ArgumentException("Invalid status.")
    };

    public static string ToApiValue(TaskStatus status) => status switch
    {
        TaskStatus.Todo => "todo",
        TaskStatus.InProgress => "in_progress",
        TaskStatus.Done => "done",
        _ => throw new ArgumentOutOfRangeException(nameof(status))
    };

}