using TaskManagerApp.Data;

namespace TaskManagerApp.Repositories
{
    public interface ITaskTagRepository
    {
        Task<TaskTag?> GetTaskTagAsync(int taskItemId, int tagId);
        Task AddTaskTagAsync(TaskTag taskTag);
        Task RemoveTaskTagAsync(int taskItemId, int tagId);
        Task<List<TaskTag>> GetTagsForTaskAsync(int taskItemId);
        Task<List<TaskTag>> GetTasksForTagAsync(int tagId);
    }
}
