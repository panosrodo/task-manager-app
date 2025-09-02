using TaskManagerApp.Core.Enums;
using TaskManagerApp.DTO;

namespace TaskManagerApp.Services
{
    public interface ITaskItemService
    {
        Task<TaskItemReadOnlyDTO?> CreateTaskItemAsync(TaskItemCreateDTO dto);
        Task<bool> UpdateTaskItemAsync(TaskItemUpdateDTO dto);
        Task<bool> DeleteTaskItemAsync(int taskItemId);
        Task<TaskItemReadOnlyDTO?> GetTaskItemAsync(int taskItemId);
        Task<List<TaskItemReadOnlyDTO>> GetTaskItemsByProjectAsync(int projectId);
        Task<List<TaskItemReadOnlyDTO>> GetTaskItemsByAssigneeAsync(int assigneeId);
        Task<List<TaskItemReadOnlyDTO>> GetTaskItemsByStatusAsync(TaskItemStatus status);
        Task<List<TaskItemReadOnlyDTO>> GetTaskItemsByPriorityAsync(int priority);
    }
}