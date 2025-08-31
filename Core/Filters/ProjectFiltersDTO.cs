using TaskManagerApp.Core.Enums;

namespace TaskManagerApp.Core.Filters
{
    public class ProjectFiltersDTO
    {
        public string? Title { get; set; }
        public int? OwnerId { get; set; }
        public TaskItemStatus? Status { get; set; }
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
        public DateTime? EndDateFrom { get; set; }
        public DateTime? EndDateTo { get; set; }
    }
}
