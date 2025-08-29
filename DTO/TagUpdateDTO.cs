using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.DTO
{
    public class TagUpdateDTO
    {
        [Required(ErrorMessage = "TagId is required.")]
        public int TagId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }
    }
}
