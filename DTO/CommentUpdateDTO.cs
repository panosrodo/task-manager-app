using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.DTO
{
    public class CommentUpdateDTO
    {
        [Required]
        public int CommentId { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "The comment must be between 1 and 1000 characters long.")]
        public string Text { get; set; } = string.Empty;
    }
}