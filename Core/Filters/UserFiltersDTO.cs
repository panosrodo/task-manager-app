using TaskManagerApp.Core.Enums;

namespace TaskManagerApp.Core.Filters
{
    public class UserFiltersDTO
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public UserRole? Role { get; set; }
    }
}
