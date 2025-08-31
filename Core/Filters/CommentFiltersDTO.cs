namespace TaskManagerApp.Core.Filters
{
    public class CommentFiltersDTO
    {
        public int? TaskId { get; set; }
        public int? AuthorId { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public string? Content { get; set; }
    }
}
