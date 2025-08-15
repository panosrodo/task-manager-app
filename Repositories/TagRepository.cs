using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagerApp.Data;

namespace TaskManagerApp.Repositories
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(TaskManagerDbContext context) : base(context)
        {
        }

        public async Task<Tag?> GetTagAsync(int id)
        {
            return await context.Tags
                .Include(t => t.TaskTags)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tag?> UpdateTagAsync(int id, Tag tag)
        {
            var existingTag = await context.Tags.FindAsync(id);

            if (existingTag == null)
                return null;

            existingTag.Name = tag.Name;

            await context.SaveChangesAsync();
            return existingTag;
        }

        public async Task<List<Tag>> GetAllTagsFilteredPaginatedAsync(int pageNumber, int pageSize, List<Expression<Func<Tag, bool>>> predicates)
        {
            IQueryable<Tag> query = context.Tags.Include(t => t.TaskTags);

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
