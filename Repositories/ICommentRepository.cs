using System.Linq.Expressions;
using TaskManagerApp.Data;

namespace TaskManagerApp.Repositories
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task<Comment?> GetCommentAsync(int id);
        Task<Comment?> UpdateCommentAsync(int id, Comment comment);
        Task<List<Comment>> GetAllCommentsFilteredPaginatedAsync(int pageNumber, int pageSize,
            List<Expression<Func<Comment, bool>>> predicates);
    }
}
