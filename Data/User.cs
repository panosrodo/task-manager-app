using System.ComponentModel.DataAnnotations;
using TaskManagerApp.Core.Enums;


namespace TaskManagerApp.Data
{
    public class User : BaseEntity
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public UserRole Role { get; set; } = UserRole.User;

        // Navigation properties
        public ICollection<Project>? OwnedProjects { get; set; } = new List<Project>();
        public ICollection<TaskItem>? AssignedTaskItems { get; set; } = new List<TaskItem>();
        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
    }
}
