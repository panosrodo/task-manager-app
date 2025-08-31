using System.ComponentModel.DataAnnotations;
using TaskManagerApp.Core.Enums;

namespace TaskManagerApp.DTO
{
    public class TaskItemUpdateDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "TaskItemId must be a positive number.")]
        public int? TaskItemId { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters.")]
        public string? Title { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        [Range(1, 5, ErrorMessage = "Priority must be between 1 and 5.")]
        public int? Priority { get; set; }

        [EnumDataType(typeof(TaskItemStatus), ErrorMessage = "Invalid status value.")]
        public TaskItemStatus? Status { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ProjectId must be a positive number.")]
        public int? ProjectId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "AssigneeId must be a positive number.")]
        public int? AssigneeId { get; set; }
        public List<int>? TagIds { get; set; }
    }
}