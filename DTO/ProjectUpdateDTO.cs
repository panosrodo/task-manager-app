using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.DTO
{
    public class ProjectUpdateDTO
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string? Name { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "OwnerId must be a positive number.")]
        public int? OwnerId { get; set; }
    }
}
