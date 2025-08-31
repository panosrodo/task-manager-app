using System.Linq.Expressions;
using TaskManagerApp.Data;

namespace TaskManagerApp.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetUserAsync(string username, string password);
        Task<User?> UpdateUserAsync(int id, User user);
        Task<User?> GetByUsernameAsync(string username);
        Task<List<User>> GetAllUsersFilteredPaginatedAsync(int pageNumber, int pageSize,
            List<Expression<Func<User, bool>>> predicates);
    }
}
