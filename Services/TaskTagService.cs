using TaskManagerApp.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace TaskManagerApp.Services
{
    public class TaskTagService : ITaskTagService
    {
        private readonly TaskManagerDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskTagService> _logger;

        public TaskTagService(
            TaskManagerDbContext context,
            IMapper mapper,
            ILogger<TaskTagService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<TaskTag>> GetAllAsync()
        {
            try
            {
                return await _context.TaskTags.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllAsync");
                throw;
            }
        }

        public async Task<TaskTag?> GetByIdAsync(int taskItemId, int tagId)
        {
            try
            {
                return await _context.TaskTags
                    .FirstOrDefaultAsync(tt => tt.TaskItemId == taskItemId && tt.TagId == tagId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetByIdAsync for TaskItemId {TaskItemId} and TagId {TagId}", taskItemId, tagId);
                throw;
            }
        }

        public async Task<TaskTag> CreateAsync(TaskTag taskTag)
        {
            try
            {
                _context.TaskTags.Add(taskTag);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Created TaskTag TaskItemId {TaskItemId} with TagId {TagId}", taskTag.TaskItemId, taskTag.TagId);
                return taskTag;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateAsync for TaskItemId {TaskItemId} and TagId {TagId}", taskTag.TaskItemId, taskTag.TagId);
                throw;
            }
        }

        public async Task<TaskTag> UpdateAsync(TaskTag taskTag)
        {
            try
            {
                var existing = await _context.TaskTags
                    .FirstOrDefaultAsync(tt => tt.TaskItemId == taskTag.TaskItemId && tt.TagId == taskTag.TagId);
                if (existing == null)
                    throw new Exception("TaskTag not found");

                _context.Entry(existing).CurrentValues.SetValues(taskTag);

                await _context.SaveChangesAsync();
                _logger.LogInformation("Updated TaskTag TaskItemId {TaskItemId} with TagId {TagId}", taskTag.TaskItemId, taskTag.TagId);
                return existing;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateAsync for TaskItemId {TaskItemId} and TagId {TagId}", taskTag.TaskItemId, taskTag.TagId);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int taskItemId, int tagId)
        {
            try
            {
                var entity = await _context.TaskTags
                    .FirstOrDefaultAsync(tt => tt.TaskItemId == taskItemId && tt.TagId == tagId);
                if (entity == null)
                    return false;

                _context.TaskTags.Remove(entity);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deleted TaskTag TaskItemId {TaskItemId} with TagId {TagId}", taskItemId, tagId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteAsync for TaskItemId {TaskItemId} and TagId {TagId}", taskItemId, tagId);
                throw;
            }
        }

        public async Task<List<TaskTag>> GetAllTaskTagsFilteredAsync(int pageNumber, int pageSize, int? taskItemId, int? tagId)
        {
            try
            {
                var query = _context.TaskTags.AsQueryable();

                if (taskItemId.HasValue)
                    query = query.Where(tt => tt.TaskItemId == taskItemId.Value);
                if (tagId.HasValue)
                    query = query.Where(tt => tt.TagId == tagId.Value);

                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllTaskTagsFilteredAsync");
                throw;
            }
        }
    }
}