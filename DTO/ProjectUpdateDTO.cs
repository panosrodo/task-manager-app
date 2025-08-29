using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.DTO
{
    public class ProjectUpdateDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "OwnerId is required.")]
        public int OwnerId { get; set; }
    }
}
