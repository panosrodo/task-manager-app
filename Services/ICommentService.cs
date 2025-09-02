using TaskManagerApp.DTO;
using TaskManagerApp.Core.Filters;

namespace TaskManagerApp.Services
{
    public interface ICommentService
    {
        Task<CommentReadOnlyDTO?> CreateCommentAsync(CommentCreateDTO commentCreateDTO);
        Task<bool> UpdateCommentAsync(CommentUpdateDTO commentUpdateDTO);
        Task<bool> DeleteCommentAsync(int commentId);
        Task<CommentReadOnlyDTO?> GetCommentAsync(int commentId);
        Task<List<CommentReadOnlyDTO>> GetCommentsByTaskItemAsync(int taskItemId);
        Task<List<CommentReadOnlyDTO>> GetCommentsFilteredAsync(CommentFiltersDTO filtersDTO, int pageNumber, int pageSize);
    }
}