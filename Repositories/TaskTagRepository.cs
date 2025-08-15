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
