using Microsoft.EntityFrameworkCore;
using YardView.TaskManager.Server.Models;

namespace YardView.TaskManager.Server.Data
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>(e =>
            {
                e.ToTable("Tasks");

                e.HasKey(e => e.Id);

                e.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                e.Property(x => x.Description)
                    .HasMaxLength(1000);

                e.Property(x => x.Status)
                    .HasConversion<string>()
                    .IsRequired();

                e.Property(x => x.CreatedAt)
                    .IsRequired();
            });
        }
    }
}
