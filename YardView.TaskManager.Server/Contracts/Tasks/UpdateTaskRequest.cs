namespace YardView.TaskManager.Server.Contracts.Tasks;

public class UpdateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "todo";
    public DateTime? DueDate { get; set; }
}