using System.ComponentModel.DataAnnotations;
using TaskManagerApp.Core.Enums;

namespace TaskManagerApp.DTO
{
    public class TaskItemCreateDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Due date is required.")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Range(1, 5, ErrorMessage = "Priority must be between 1 and 5.")]
        public int Priority { get; set; }

        [Required(ErrorMessage = "ProjectId is required.")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "AssigneeId is required.")]
        public int AssigneeId { get; set; }

        [EnumDataType(typeof(TaskItemStatus), ErrorMessage = "Invalid status value.")]
        public TaskItemStatus Status { get; set; } = TaskItemStatus.Open;

        public List<int>? TagIds { get; set; }
    }
}