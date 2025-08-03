namespace TaskManagerApp.Data
{
    public class Comment : BaseEntity
    {
        public string Text { get; set; } = null!;

        public int UserId { get; set; }
        public int TaskItemId { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public TaskItem? TaskItem { get; set; }
    }
}
