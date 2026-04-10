using Microsoft.EntityFrameworkCore;
using YardView.TaskManager.Server.Contracts.Tasks;
using YardView.TaskManager.Server.Data;
using YardView.TaskManager.Server.Extensions;

namespace YardView.TaskManager.Server.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskResponse>> GetTasksAsync(string? status, CancellationToken cancellationToken);
    Task<TaskResponse?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<TaskResponse> CreateAsync(CreateTaskRequest request, CancellationToken cancellationToken);
    Task<TaskResponse?> UpdateAsync(int id, UpdateTaskRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}

public class TaskService : ITaskService
{
    private readonly AppDbContext _dbContext;
    public TaskService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TaskResponse>> GetTasksAsync(string? status, CancellationToken cancellationToken)
    {
        var query = _dbContext.Tasks.AsNoTracking();

        if (!string.IsNullOrEmpty(status))
        {
            var enumStatus = status.ToEnum<Models.TaskStatus>();
            if (enumStatus.HasValue)
            {
                query = query.Where(t => t.Status == enumStatus.Value);
            }
        }

        return await query.Select(t => new TaskResponse
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status.ToString(),
            CreatedAt = t.CreatedAt
        }).ToListAsync(cancellationToken);

    }

    public async Task<TaskResponse?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var task = await _dbContext.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        return task == null ? null : new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            CreatedAt = task.CreatedAt
        };
    }

    public async Task<TaskResponse> CreateAsync(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var task = new Models.TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Status = request.Status.ToEnum<Models.TaskStatus>() ?? Models.TaskStatus.Todo,
            CreatedAt = DateTime.UtcNow
        };
        
        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync(cancellationToken);
    
        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            CreatedAt = task.CreatedAt
        };
    }

    public async Task<TaskResponse?> UpdateAsync(int id, UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (task == null)
        {
            return null;
        }

        task.Title = request.Title;
        task.Status = request.Status.ToEnum<Models.TaskStatus>() ?? task.Status;
        task.Description = request.Description;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            CreatedAt = task.CreatedAt
        };
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (task == null)
        {
            return false;
        }

        _dbContext.Tasks.Remove(task);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
