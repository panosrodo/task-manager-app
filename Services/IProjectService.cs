using TaskManagerApp.Core.Filters;
using TaskManagerApp.Data;
using TaskManagerApp.DTO;

namespace TaskManagerApp.Services
{
    public interface IProjectService
    {
        Task<Project?> CreateProjectAsync(ProjectCreateDTO projectCreateDTO);
        Task<bool> UpdateProjectAsync(int projectId, ProjectUpdateDTO projectUpdateDTO);
        Task<bool> DeleteProjectAsync(int projectId);
        Task<Project?> GetProjectAsync(int projectId);
        Task<List<Project>> GetAllProjectsAsync();
        Task<List<Project>> GetAllProjectsFilteredAsync(int pageNumber, int pageSize, ProjectFiltersDTO projectFiltersDTO);
        Task<List<TaskItem>> GetProjectTasksAsync(int projectId);
    }
}