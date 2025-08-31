namespace TaskManagerApp.Core.Filters
{
    public class TaskItemFiltersDTO
    {
        public string? Title { get; set; }
        public TaskStatus? Status { get; set; }
        public int? AssignedUserId { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
    }
}
