using TaskManagerApp.Data;

namespace TaskManagerApp.Services
{
    public interface ITaskTagService
    {
        Task<IEnumerable<TaskTag>> GetAllAsync();
        Task<TaskTag?> GetByIdAsync(int taskItemId, int tagId);
        Task<TaskTag> CreateAsync(TaskTag taskTag);
        Task<TaskTag> UpdateAsync(TaskTag taskTag);
        Task<bool> DeleteAsync(int taskItemId, int tagId);
        Task<List<TaskTag>> GetAllTaskTagsFilteredAsync(int pageNumber, int pageSize, int? taskItemId, int? tagId);
    }
}
