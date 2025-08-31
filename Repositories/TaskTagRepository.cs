using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;

namespace TaskManagerApp.Repositories
{
    public class TaskTagRepository : ITaskTagRepository
    {
        private readonly TaskManagerDbContext context;

        public TaskTagRepository(TaskManagerDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(TaskTag entity)
        {
            await context.TaskTags.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<TaskTag> entities)
        {
            await context.TaskTags.AddRangeAsync(entities);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskTag entity)
        {
            context.TaskTags.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await Task.FromResult(false);
        }

        public async Task<TaskTag?> GetAsync(int id)
        {
            return await Task.FromResult<TaskTag?>(null);
        }

        public async Task<IEnumerable<TaskTag>> GetAllAsync()
        {
            return await context.TaskTags
                .Include(tt => tt.TaskItem)
                .Include(tt => tt.Tag)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await context.TaskTags.CountAsync();
        }

        public async Task<TaskTag?> GetTaskTagAsync(int taskItemId, int tagId)
        {
            return await context.TaskTags
                .Include(tt => tt.TaskItem)
                .Include(tt => tt.Tag)
                .FirstOrDefaultAsync(tt => tt.TaskItemId == taskItemId && tt.TagId == tagId);
        }

        public async Task AddTaskTagAsync(TaskTag taskTag)
        {
            context.TaskTags.Add(taskTag);
            await context.SaveChangesAsync();
        }

        public async Task RemoveTaskTagAsync(int taskItemId, int tagId)
        {
            var taskTag = await context.TaskTags
                .FirstOrDefaultAsync(tt => tt.TaskItemId == taskItemId && tt.TagId == tagId);
            if (taskTag != null)
            {
                context.TaskTags.Remove(taskTag);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<TaskTag>> GetTagsForTaskAsync(int taskItemId)
        {
            return await context.TaskTags
                .Where(tt => tt.TaskItemId == taskItemId)
                .Include(tt => tt.Tag)
                .ToListAsync();
        }

        public async Task<List<TaskTag>> GetTasksForTagAsync(int tagId)
        {
            return await context.TaskTags
                .Where(tt => tt.TagId == tagId)
                .Include(tt => tt.TaskItem)
                .ToListAsync();
        }
    }
}