namespace TaskManagerApp.Data
{
    public class TaskTag
    {
        public int TaskItemId { get; set; }
        public int TagId { get; set; }

        // Navigation properties
        public TaskItem? TaskItem { get; set; }
        public Tag? Tag { get; set; }
    }
}
