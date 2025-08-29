namespace TaskManagerApp.Filters
{
    public class CommentFilter : GenericFilter
    {
        public int? TaskItemId { get; set; }
        public int? UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? TextContains { get; set; }
    }
}
