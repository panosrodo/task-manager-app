using System.Linq.Expressions;
using TaskManagerApp.Data;

namespace TaskManagerApp.Repositories
{
    public interface IProjectRepository
    {
        Task<Project?> GetProjectAsync(int projectId);
        Task<Project?> UpdateProjectAsync(int id, Project project);
        Task<List<Project>> GetAllProjectsFilteredPaginatedAsync(int pageNumber, int pageSize,
            List<Expression<Func<Project, bool>>> predicates);
    }
}
