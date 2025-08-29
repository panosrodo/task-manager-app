using System.ComponentModel.DataAnnotations;

namespace TaskManagerApp.DTO
{
    public class TaskTagCreateDTO
    {
        [Required]
        public int TaskItemId { get; set; }

        [Required]
        public int TagId { get; set; }
    }
}
