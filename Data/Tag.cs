namespace TaskManagerApp.Data
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; } = null!;

        // Navigation property
        public ICollection<TaskTag>? TaskTags { get; set; }
    }
}
