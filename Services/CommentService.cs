using TaskManagerApp.Data;
using TaskManagerApp.DTO;
using TaskManagerApp.Core.Filters;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace TaskManagerApp.Services
{
    public class CommentService : ICommentService
    {
        private readonly TaskManagerDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CommentService> _logger;

        public CommentService(
            TaskManagerDbContext context,
            IMapper mapper,
            ILogger<CommentService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CommentReadOnlyDTO?> CreateCommentAsync(CommentCreateDTO commentCreateDTO)
        {
            try
            {
                var comment = _mapper.Map<Comment>(commentCreateDTO);
                comment.InsertedAt = DateTime.UtcNow;
                comment.ModifiedAt = DateTime.UtcNow;

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                return _mapper.Map<CommentReadOnlyDTO>(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateCommentAsync for TaskItemId {TaskItemId}", commentCreateDTO.TaskItemId);
                throw;
            }
        }

        public async Task<bool> UpdateCommentAsync(CommentUpdateDTO commentUpdateDTO)
        {
            try
            {
                if (!commentUpdateDTO.CommentId.HasValue)
                    return false;

                var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentUpdateDTO.CommentId.Value);
                if (comment == null)
                    return false;

                if (!string.IsNullOrEmpty(commentUpdateDTO.Text))
                    comment.Text = commentUpdateDTO.Text;

                comment.ModifiedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateCommentAsync for CommentId {CommentId}", commentUpdateDTO.CommentId);
                throw;
            }
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            try
            {
                var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
                if (comment == null)
                    return false;

                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteCommentAsync for CommentId {CommentId}", commentId);
                throw;
            }
        }

        public async Task<CommentReadOnlyDTO?> GetCommentAsync(int commentId)
        {
            try
            {
                var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
                return comment == null ? null : _mapper.Map<CommentReadOnlyDTO>(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCommentAsync for CommentId {CommentId}", commentId);
                throw;
            }
        }

        public async Task<List<CommentReadOnlyDTO>> GetCommentsByTaskItemAsync(int taskItemId)
        {
            try
            {
                var comments = await _context.Comments
                    .Where(c => c.TaskItemId == taskItemId)
                    .OrderByDescending(c => c.InsertedAt)
                    .ToListAsync();

                return _mapper.Map<List<CommentReadOnlyDTO>>(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCommentsByTaskItemAsync for TaskItemId {TaskItemId}", taskItemId);
                throw;
            }
        }

        public async Task<List<CommentReadOnlyDTO>> GetCommentsFilteredAsync(CommentFiltersDTO filtersDTO, int pageNumber, int pageSize)
        {
            try
            {
                var query = _context.Comments.AsQueryable();

                if (filtersDTO != null)
                {
                    if (filtersDTO.TaskId.HasValue)
                        query = query.Where(c => c.TaskItemId == filtersDTO.TaskId.Value);
                    if (filtersDTO.AuthorId.HasValue)
                        query = query.Where(c => c.UserId == filtersDTO.AuthorId.Value);
                    if (filtersDTO.CreatedDateFrom.HasValue)
                        query = query.Where(c => c.InsertedAt >= filtersDTO.CreatedDateFrom.Value);
                    if (filtersDTO.CreatedDateTo.HasValue)
                        query = query.Where(c => c.InsertedAt <= filtersDTO.CreatedDateTo.Value);
                    if (!string.IsNullOrEmpty(filtersDTO.Content))
                        query = query.Where(c => c.Text.Contains(filtersDTO.Content));
                }

                query = query.OrderByDescending(c => c.InsertedAt)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize);

                var comments = await query.ToListAsync();
                return _mapper.Map<List<CommentReadOnlyDTO>>(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetCommentsFilteredAsync");
                throw;
            }
        }
    }
}