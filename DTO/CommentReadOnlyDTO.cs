namespace TaskManagerApp.DTO
{
    public class CommentReadOnlyDTO
    {
        public int CommentId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int TaskItemId { get; set; }
        public int UserId { get; set; }
    }
}
