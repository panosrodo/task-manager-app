namespace TaskManagerApp.DTO
{
    public class ProjectReadOnlyDTO
    {
        public int ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int OwnerId { get; set; }
    }
}
