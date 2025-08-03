using Microsoft.EntityFrameworkCore;

namespace TaskManagerApp.Data
{
    public class TaskManagerDbContext : DbContext
    {
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TaskTag> TaskTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(u => u.Username)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(u => u.PasswordHash)
                      .IsRequired();

                entity.Property(u => u.Role)
                      .IsRequired();

                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.Username).IsUnique();

                entity.Property(u => u.InsertedAt)
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(u => u.ModifiedAt)
                      .ValueGeneratedOnAddOrUpdate()
                      .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Projects");
                entity.Property(p => p.Name)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(p => p.Description)
                      .IsRequired();

                entity.Property(p => p.InsertedAt)
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(p => p.ModifiedAt)
                      .ValueGeneratedOnAddOrUpdate()
                      .HasDefaultValueSql("GETDATE()");

                modelBuilder.Entity<Project>()
                            .HasOne(p => p.Owner)
                            .WithMany(u => u.OwnedProjects)
                            .HasForeignKey(p => p.OwnerId)
                            .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.ToTable("TaskItems");

                entity.Property(t => t.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(t => t.Description)
                      .IsRequired();

                entity.Property(t => t.Priority)
                     .IsRequired();

                entity.Property(t => t.Status)
                      .IsRequired();

                entity.Property(t => t.InsertedAt)
                     .ValueGeneratedOnAdd()
                     .HasDefaultValueSql("GETDATE()");

                entity.Property(t => t.ModifiedAt)
                     .ValueGeneratedOnAddOrUpdate()
                     .HasDefaultValueSql("GETDATE()");

                modelBuilder.Entity<TaskItem>()
                            .HasOne(t => t.Assignee)
                            .WithMany(u => u.AssignedTaskItems)
                            .HasForeignKey(t => t.AssigneeId)
                            .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comments");

                entity.Property(c => c.Text)
                      .IsRequired();

                entity.Property(c => c.InsertedAt)
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(c => c.ModifiedAt)
                      .ValueGeneratedOnAddOrUpdate()
                      .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tags");

                entity.Property(tg => tg.Name)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(tg => tg.InsertedAt)
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("GETDATE()"); 

                entity.Property(tg => tg.ModifiedAt)
                      .ValueGeneratedOnAddOrUpdate()
                      .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<TaskTag>(entity =>
            {
                entity.ToTable("TaskTags");
                entity.HasKey(tt => new { tt.TaskItemId, tt.TagId });

                entity.HasOne(tt => tt.TaskItem)
                      .WithMany(t => t.TaskTags)
                      .HasForeignKey(tt => tt.TaskItemId);

                entity.HasOne(tt => tt.Tag)
                      .WithMany(t => t.TaskTags)
                      .HasForeignKey(tt => tt.TagId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
