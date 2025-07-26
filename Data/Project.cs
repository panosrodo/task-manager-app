namespace TaskManagerApp.Data
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public int OwnerId { get; set; }

        // Navigation properties
        public User? Owner { get; set; }
        public ICollection<Task>? Tasks { get; set; }
    }
}
