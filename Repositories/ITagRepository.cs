using System.Linq.Expressions;
using TaskManagerApp.Data;

namespace TaskManagerApp.Repositories
{
    public interface ITagRepository
    {
        Task<Tag?> GetTagAsync(int id);
        Task<Tag?> UpdateTagAsync(int id, Tag tag);
        Task<List<Tag>> GetAllTagsFilteredPaginatedAsync(int pageNumber, int pageSize,
            List<Expression<Func<Tag, bool>>> predicates);
    }
}
