using System.Linq.Expressions;
using TaskManagerApp.Data;

namespace TaskManagerApp.Repositories
{
    public interface ITaskItemRepository : IBaseRepository<TaskItem>
    {
        Task<TaskItem?> GetTaskAsync(int taskId);
        Task<TaskItem?> UpdateTaskItemAsync(int id, TaskItem taskItem);
        Task<List<TaskItem>> GetAllTaskItemsFilteredPaginatedAsync(int pageNumber, int pageSize,
            List<Expression<Func<TaskItem, bool>>> predicates);
    }
}
