using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagerApp.Data;

namespace TaskManagerApp.Repositories
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        public ProjectRepository(TaskManagerDbContext context) : base(context)
        {
        }

        public async Task<Project?> GetProjectAsync(int projectId)
        {
            return await context.Projects
                .Include(p => p.TaskItems)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<Project?> UpdateProjectAsync(int id, Project project)
        {
            var existingProject = context.Projects.Find(id);

            if (existingProject == null)
            {
                return null;
            }

            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.OwnerId = project.OwnerId;

            await context.SaveChangesAsync();
            return existingProject;

        }

        public async Task<List<Project>> GetAllProjectsFilteredPaginatedAsync(int pageNumber, int pageSize, List<Expression<Func<Project, bool>>> predicates)
        {
            IQueryable<Project> query = context.Projects.Include(p => p.TaskItems);


            if (predicates != null && predicates.Any())
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            int skip = (pageNumber - 1) * pageSize;
            return await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
