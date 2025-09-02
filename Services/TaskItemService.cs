using TaskManagerApp.Data;
using TaskManagerApp.DTO;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using TaskManagerApp.Core.Enums;

namespace TaskManagerApp.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly TaskManagerDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskItemService> _logger;

        public TaskItemService(
            TaskManagerDbContext context,
            IMapper mapper,
            ILogger<TaskItemService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TaskItemReadOnlyDTO?> CreateTaskItemAsync(TaskItemCreateDTO dto)
        {
            try
            {
                var entity = _mapper.Map<TaskItem>(dto);
                entity.InsertedAt = DateTime.UtcNow;
                entity.ModifiedAt = DateTime.UtcNow;

                if (dto.TagIds != null && dto.TagIds.Any())
                {
                    entity.TaskTags = dto.TagIds.Select(tagId => new TaskTag
                    {
                        TagId = tagId,
                        TaskItem = entity
                    }).ToList();
                }

                entity.Priority = (TaskPriority)dto.Priority;

                _context.TaskItems.Add(entity);
                await _context.SaveChangesAsync();

                return _mapper.Map<TaskItemReadOnlyDTO>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateTaskItemAsync (ProjectId: {ProjectId})", dto.ProjectId);
                throw;
            }
        }

        public async Task<bool> UpdateTaskItemAsync(TaskItemUpdateDTO dto)
        {
            try
            {
                if (!dto.TaskItemId.HasValue)
                    return false;

                var entity = await _context.TaskItems
                    .Include(t => t.TaskTags)
                    .FirstOrDefaultAsync(t => t.Id == dto.TaskItemId.Value);

                if (entity == null)
                    return false;

                if (!string.IsNullOrEmpty(dto.Title))
                    entity.Title = dto.Title;
                if (!string.IsNullOrEmpty(dto.Description))
                    entity.Description = dto.Description;
                if (dto.DueDate.HasValue)
                    entity.DueDate = dto.DueDate.Value;
                if (dto.Priority.HasValue)
                    entity.Priority = (TaskPriority)dto.Priority.Value;
                if (dto.Status.HasValue)
                    entity.Status = dto.Status.Value;
                if (dto.ProjectId.HasValue)
                    entity.ProjectId = dto.ProjectId.Value;
                if (dto.AssigneeId.HasValue)
                    entity.AssigneeId = dto.AssigneeId.Value;

                entity.ModifiedAt = DateTime.UtcNow;

                if (dto.TagIds != null)
                {
                    entity.TaskTags?.Clear();
                    entity.TaskTags = dto.TagIds.Select(tagId => new TaskTag
                    {
                        TagId = tagId,
                        TaskItemId = entity.Id
                    }).ToList();
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateTaskItemAsync (TaskItemId: {TaskItemId})", dto.TaskItemId);
                throw;
            }
        }

        public async Task<bool> DeleteTaskItemAsync(int taskItemId)
        {
            try
            {
                var entity = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == taskItemId);
                if (entity == null)
                    return false;

                _context.TaskItems.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteTaskItemAsync (TaskItemId: {TaskItemId})", taskItemId);
                throw;
            }
        }

        public async Task<TaskItemReadOnlyDTO?> GetTaskItemAsync(int taskItemId)
        {
            try
            {
                var entity = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == taskItemId);
                return entity == null ? null : _mapper.Map<TaskItemReadOnlyDTO>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTaskItemAsync (TaskItemId: {TaskItemId})", taskItemId);
                throw;
            }
        }

        public async Task<List<TaskItemReadOnlyDTO>> GetTaskItemsByProjectAsync(int projectId)
        {
            try
            {
                var items = await _context.TaskItems
                    .Where(t => t.ProjectId == projectId)
                    .OrderByDescending(t => t.InsertedAt)
                    .ToListAsync();

                return _mapper.Map<List<TaskItemReadOnlyDTO>>(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTaskItemsByProjectAsync (ProjectId: {ProjectId})", projectId);
                throw;
            }
        }

        public async Task<List<TaskItemReadOnlyDTO>> GetTaskItemsByAssigneeAsync(int assigneeId)
        {
            try
            {
                var items = await _context.TaskItems
                    .Where(t => t.AssigneeId == assigneeId)
                    .OrderByDescending(t => t.InsertedAt)
                    .ToListAsync();

                return _mapper.Map<List<TaskItemReadOnlyDTO>>(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTaskItemsByAssigneeAsync (AssigneeId: {AssigneeId})", assigneeId);
                throw;
            }
        }

        public async Task<List<TaskItemReadOnlyDTO>> GetTaskItemsByStatusAsync(TaskItemStatus status)
        {
            try
            {
                var items = await _context.TaskItems
                    .Where(t => t.Status == status)
                    .OrderByDescending(t => t.InsertedAt)
                    .ToListAsync();

                return _mapper.Map<List<TaskItemReadOnlyDTO>>(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTaskItemsByStatusAsync (Status: {Status})", status);
                throw;
            }
        }

        public async Task<List<TaskItemReadOnlyDTO>> GetTaskItemsByPriorityAsync(int priority)
        {
            try
            {
                var items = await _context.TaskItems
                    .Where(t => (int)t.Priority == priority)
                    .OrderByDescending(t => t.InsertedAt)
                    .ToListAsync();

                return _mapper.Map<List<TaskItemReadOnlyDTO>>(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTaskItemsByPriorityAsync (Priority: {Priority})", priority);
                throw;
            }
        }
    }
}