namespace TaskManagerApp.Data
{
    public class TaskTag
    {
        public int TaskId { get; set; }
        public int TagId { get; set; }

        // Navigation properties
        public Task? Task { get; set; }
        public Tag? Tag { get; set; }
    }
}
