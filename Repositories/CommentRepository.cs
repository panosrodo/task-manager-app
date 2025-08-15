using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using TaskManagerApp.Data;

namespace TaskManagerApp.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(TaskManagerDbContext context) : base(context)
        {
        }

        public async Task<Comment?> GetCommentAsync(int id)
        {
            return await context.Comments
                .Include(c => c.User)
                .Include(c => c.TaskItem)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment?> UpdateCommentAsync(int id, Comment comment)
        {
            var existingComment = await context.Comments.FindAsync(id);

            if (existingComment == null)
                return null;

            existingComment.Text = comment.Text;
            existingComment.UserId = comment.UserId;
            existingComment.TaskItemId = comment.TaskItemId;

            await context.SaveChangesAsync();
            return existingComment;
        }

        public async Task<List<Comment>> GetAllCommentsFilteredPaginatedAsync(int pageNumber, int pageSize, List<Expression<Func<Comment, bool>>> predicates)
        {
            IQueryable<Comment> query = context.Comments
                .Include(c => c.User)
                .Include(c => c.TaskItem);

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
