using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.DTO
{
    public class CommentCreateDTO
    {
        [Required]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "The comment must be between 1 and 1000 characters long.")]
        public string Text { get; set; } = string.Empty;

        [Required]
        public int TaskItemId { get; set; }

        [Required]
        public int UserId { get; set; } // Σημείωση: Το UserId προς το παρόν έρχεται από το DTO.
                                        // Αν προσθέσουμε authentication, το UserId θα πρέπει
                                        // να παίρνεται από το authentication context του backend.
    }
}