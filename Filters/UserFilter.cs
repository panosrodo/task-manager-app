namespace TaskManagerApp.Filters
{
    public class UserFilter : GenericFilter
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
