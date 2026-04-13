using Microsoft.EntityFrameworkCore;
using YardView.TaskManager.Server.Models;

namespace YardView.TaskManager.Server.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            await context.Database.MigrateAsync();

            if (await context.Tasks.AnyAsync())
                return;

            var tasks = new List<TaskItem>
            {
                new TaskItem
                {
                    Title = "Setup project",
                    Description = "Initialize API and Angular app",
                    Status = Models.TaskStatus.Done,
                    CreatedAt = DateTime.UtcNow,
                },
                new TaskItem
                {
                    Title = "Past Due Task",
                    Description = "This task is late and should be completed ASAP.",
                    Status = Models.TaskStatus.Todo,
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    DueDate = DateTime.UtcNow.AddDays(-1)
                },
                new TaskItem
                {
                    Title = "Build API",
                    Description = "Implement CRUD endpoints",
                    Status = Models.TaskStatus.InProgress,
                    CreatedAt = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(3)
                },
                new TaskItem
                {
                    Title = "Finish UI",
                    Description = "Connect frontend to API",
                    Status = Models.TaskStatus.Todo,
                    CreatedAt = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(5)
                }
            };

            await context.Tasks.AddRangeAsync(tasks);
            await context.SaveChangesAsync();
        }
    }
}
