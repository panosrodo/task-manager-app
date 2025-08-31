using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.DTO
{
    public class CommentUpdateDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "CommentId must be a positive number.")]
        public int? CommentId { get; set; }

        [StringLength(1000, MinimumLength = 1, ErrorMessage = "The comment must be between 1 and 1000 characters long.")]
        public string? Text { get; set; }
    }
}