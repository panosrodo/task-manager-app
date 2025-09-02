using TaskManagerApp.Core.Filters;
using TaskManagerApp.Data;
using TaskManagerApp.DTO;
using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Repositories;
using AutoMapper;

namespace TaskManagerApp.Services
{
    public class UserService : IUserService
    {
        private readonly TaskManagerDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(
            TaskManagerDbContext context,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<UserService> logger)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<User?> VerifyAndGetUserAsync(UserLoginDTO credentials)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == credentials.Username);

                if (user != null && user.PasswordHash == credentials.Password)
                {
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in VerifyAndGetUserAsync for username {Username}", credentials.Username);
                throw;
            }
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserByUsernameAsync for username {Username}", username);
                throw;
            }
        }

        public async Task<List<User>> GetAllUsersFiltered(int pageNumber, int pageSize, UserFiltersDTO userFiltersDTO)
        {
            try
            {
                var query = _context.Users.AsQueryable();

                if (userFiltersDTO != null)
                {
                    if (!string.IsNullOrEmpty(userFiltersDTO.Username))
                        query = query.Where(u => u.Username.Contains(userFiltersDTO.Username));
                    if (!string.IsNullOrEmpty(userFiltersDTO.Email))
                        query = query.Where(u => u.Email.Contains(userFiltersDTO.Email));
                    if (userFiltersDTO.Role.HasValue)
                        query = query.Where(u => u.Role == userFiltersDTO.Role.Value);
                }

                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllUsersFiltered");
                throw;
            }
        }

        public async Task<bool> UpdateUserAsync(string username, UserUpdateDTO userUpdateDTO)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                    return false;

                user.Email = userUpdateDTO.Email ?? user.Email;
                user.FirstName = userUpdateDTO.FirstName ?? user.FirstName;
                user.LastName = userUpdateDTO.LastName ?? user.LastName;

                if (!string.IsNullOrEmpty(userUpdateDTO.Password))
                    user.PasswordHash = userUpdateDTO.Password;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateUserAsync for username {Username}", username);
                throw;
            }
        }

        public async Task<User?> CreateUserAsync(UserCreateDTO userCreateDTO)
        {
            try
            {
                var user = new User
                {
                    Username = userCreateDTO.Username,
                    Email = userCreateDTO.Email,
                    FirstName = userCreateDTO.FirstName,
                    LastName = userCreateDTO.LastName,
                    PasswordHash = userCreateDTO.Password,
                    InsertedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateUserAsync for username {Username}", userCreateDTO.Username);
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                    return false;

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteUserAsync for username {Username}", username);
                throw;
            }
        }

        public async Task<List<Project>> GetUserProjectsAsync(string username)
        {
            try
            {
                return await _context.Projects
                    .Where(p => p.Owner != null && p.Owner.Username == username)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserProjectsAsync for username {Username}", username);
                throw;
            }
        }

        public async Task<List<TaskItem>> GetUserTasksAsync(string username)
        {
            try
            {
                return await _context.TaskItems
                    .Where(t => t.Assignee != null && t.Assignee.Username == username)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserTasksAsync for username {Username}", username);
                throw;
            }
        }
    }
}