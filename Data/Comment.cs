namespace TaskManagerApp.Data
{
    public class Comment : BaseEntity
    {
        public string Text { get; set; } = null!;

        public int UserId { get; set; }
        public int TaskId { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public Task? Task { get; set; }
    }
}
