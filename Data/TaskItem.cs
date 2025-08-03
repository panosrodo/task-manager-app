using TaskManagerApp.Core.Enums;
using TaskStatus = TaskManagerApp.Core.Enums.TaskStatus;

namespace TaskManagerApp.Data
{
    public class TaskItem : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DueDate { get; set; }

        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }

        public int ProjectId { get; set; }
        public int AssigneeId { get; set; }

        // Navigation properties
        public Project? Project { get; set; }
        public User? Assignee { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<TaskTag>? TaskTags { get; set; }
    }
}
