using TaskManagerApp.Core.Enums;

namespace TaskManagerApp.Filters
{
    public class TaskItemFilter : GenericFilter
    {
        public string? Title { get; set; }
        public TaskPriority? Priority { get; set; }
        public TaskItemStatus? Status { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public int? ProjectId { get; set; }
        public int? AssigneeId { get; set; }
    }
}
