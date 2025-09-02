using TaskManagerApp.Core.Filters;
using TaskManagerApp.Data;
using TaskManagerApp.DTO;

namespace TaskManagerApp.Services
{
    public interface IUserService
    {
        Task<User?> VerifyAndGetUserAsync(UserLoginDTO credentials);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<List<User>> GetAllUsersFiltered(int pageNumber, int pageSize,
            UserFiltersDTO userFiltersDTO);
        Task<bool> UpdateUserAsync(string username, UserUpdateDTO UserUpdateDTO);
        Task<User?> CreateUserAsync(UserCreateDTO UserCreateDTO);
        Task<bool> DeleteUserAsync(string username);
        Task<List<Project>> GetUserProjectsAsync(string username);
        Task<List<TaskItem>> GetUserTasksAsync(string username);
    }
}
