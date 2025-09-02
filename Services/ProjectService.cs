using TaskManagerApp.Core.Filters;
using TaskManagerApp.Data;
using TaskManagerApp.DTO;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace TaskManagerApp.Services
{
    public class ProjectService : IProjectService
    {
        private readonly TaskManagerDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(
            TaskManagerDbContext context,
            IMapper mapper,
            ILogger<ProjectService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Project?> CreateProjectAsync(ProjectCreateDTO projectCreateDTO)
        {
            try
            {
                var project = _mapper.Map<Project>(projectCreateDTO);
                project.InsertedAt = DateTime.UtcNow;
                project.ModifiedAt = DateTime.UtcNow;

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                return project;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateProjectAsync for name {Name}", projectCreateDTO.Name);
                throw;
            }
        }

        public async Task<bool> UpdateProjectAsync(int projectId, ProjectUpdateDTO projectUpdateDTO)
        {
            try
            {
                var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
                if (project == null)
                    return false;

                project.Name = projectUpdateDTO.Name ?? project.Name;
                project.Description = projectUpdateDTO.Description ?? project.Description;
                project.ModifiedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateProjectAsync for projectId {ProjectId}", projectId);
                throw;
            }
        }

        public async Task<bool> DeleteProjectAsync(int projectId)
        {
            try
            {
                var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
                if (project == null)
                    return false;

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteProjectAsync for projectId {ProjectId}", projectId);
                throw;
            }
        }

        public async Task<Project?> GetProjectByIdAsync(int projectId)
        {
            try
            {
                return await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetProjectByIdAsync for projectId {ProjectId}", projectId);
                throw;
            }
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            try
            {
                return await _context.Projects.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllProjectsAsync");
                throw;
            }
        }

        public async Task<List<Project>> GetAllProjectsFilteredAsync(int pageNumber, int pageSize, ProjectFiltersDTO projectFiltersDTO)
        {
            try
            {
                var query = _context.Projects.AsQueryable();

                if (projectFiltersDTO != null)
                {
                    if (!string.IsNullOrEmpty(projectFiltersDTO.Title))
                        query = query.Where(p => p.Name.Contains(projectFiltersDTO.Title));
                    if (projectFiltersDTO.OwnerId.HasValue)
                        query = query.Where(p => p.OwnerId == projectFiltersDTO.OwnerId.Value);
                }

                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllProjectsFilteredAsync");
                throw;
            }
        }

        public async Task<List<TaskItem>> GetProjectTasksAsync(int projectId)
        {
            try
            {
                return await _context.TaskItems
                    .Where(t => t.ProjectId == projectId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetProjectTasksAsync for projectId {ProjectId}", projectId);
                throw;
            }
        }

        public async Task<Project?> GetProjectAsync(int projectId)
        {
            try
            {
                return await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetProjectAsync for projectId {ProjectId}", projectId);
                throw;
            }
        }
    }
}