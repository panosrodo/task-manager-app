using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagerApp.Data;
using TaskManagerApp.Services;

namespace TaskManagerApp.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(TaskManagerDbContext context) : base(context)
        {
        }

        public async Task<User?> GetUserAsync(string username, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username
                   || u.Email == username);

            if (user == null)
            {
                return null;
            }

            if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        public async Task<List<User>> GetAllUsersFilteredPaginatedAsync(int pageNumber, int pageSize, List<Expression<Func<User, bool>>> predicates)
        {
            IQueryable<User> query = context.Users;

            if (predicates != null && predicates.Any())
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            int skip = (pageNumber - 1) * pageSize;
            return await query.Skip(skip).Take(pageSize).ToListAsync();
        }

        public async Task<User?> GetByUsernameAsync(string username) =>
            await context.Users.FirstOrDefaultAsync(u => u.Username == username);

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            var existingUser = await context.Users.FindAsync(id);

            if (existingUser == null)
            {
                return null;
            }

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;

            if (!string.IsNullOrEmpty(user.PasswordHash) &&
                existingUser.PasswordHash != user.PasswordHash)
            {
                existingUser.PasswordHash = PasswordHasher.HashPassword(user.PasswordHash);
            }

            await context.SaveChangesAsync();
            return existingUser;
        }
    }
}
