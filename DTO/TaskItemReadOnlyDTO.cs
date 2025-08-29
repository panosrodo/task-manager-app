using TaskManagerApp.Core.Enums;

namespace TaskManagerApp.DTO
{
    public class TaskItemReadOnlyDTO
    {
        public int TaskItemId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public int Priority { get; set; }
        public TaskItemStatus Status { get; set; }
        public int ProjectId { get; set; }
        public int AssigneeId { get; set; }
    }
}