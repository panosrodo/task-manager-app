using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.DTO
{
    public class TagUpdateDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "TagId must be a positive number.")]
        public int? TagId { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
        public string? Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }
    }
}
