using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagerApp.Data;

namespace TaskManagerApp.Repositories
{
    public class TaskItemRepository : BaseRepository<TaskItem>, ITaskItemRepository
    {
        public TaskItemRepository(TaskManagerDbContext context) : base(context)
        {
        }

        public async Task<TaskItem?> GetTaskAsync(int taskId)
        {
            return await context.TaskItems
                .Include(t => t.Project)
                .Include(t => t.Assignee)
                .Include(t => t.Comments)
                .Include(t => t.TaskTags)
                .FirstOrDefaultAsync(t => t.Id == taskId);
        }
        public async Task<TaskItem?> UpdateTaskItemAsync(int id, TaskItem taskItem)
        {
            var existingTaskItem = await context.TaskItems.FindAsync(id);

            if (existingTaskItem == null)
                return null;

            existingTaskItem.Title = taskItem.Title;
            existingTaskItem.Description = taskItem.Description;
            existingTaskItem.DueDate = taskItem.DueDate;
            existingTaskItem.Priority = taskItem.Priority;
            existingTaskItem.Status = taskItem.Status;
            existingTaskItem.ProjectId = taskItem.ProjectId;
            existingTaskItem.Assignee = taskItem.Assignee;

            await context.SaveChangesAsync();
            return existingTaskItem;
        }

        public async Task<List<TaskItem>> GetAllTaskItemsFilteredPaginatedAsync(int pageNumber, int pageSize, List<Expression<Func<TaskItem, bool>>> predicates)
        {
            IQueryable<TaskItem> query = context.TaskItems
                .Include(t => t.Project)
                .Include(t => t.Assignee)
                .Include(t => t.Comments)
                .Include(t => t.TaskTags);

            if (predicates != null && predicates.Any())
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            int skip = (pageNumber - 1) * pageSize;
            return await query.Skip(skip).Take(pageSize).ToListAsync();
        }
    }
}
