namespace TaskManagerApp.Filters
{
    public class ProjectFilter : GenericFilter
    {
        public string? Name { get; set; }
        public int? OwnerId { get; set; }
    }
}
