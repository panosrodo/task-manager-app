using TaskManagerApp.Core.Filters;
using TaskManagerApp.Data;
using TaskManagerApp.DTO;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace TaskManagerApp.Services
{
    public class TagService : ITagService
    {
        private readonly TaskManagerDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TagService> _logger;

        public TagService(
            TaskManagerDbContext context,
            IMapper mapper,
            ILogger<TagService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Tag?> TagCreateAsync(TagCreateDTO tagCreateDTO)
        {
            try
            {
                var tag = _mapper.Map<Tag>(tagCreateDTO);
                tag.InsertedAt = DateTime.UtcNow;
                tag.ModifiedAt = DateTime.UtcNow;

                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();
                return tag;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TagCreateAsync for name {Name}", tagCreateDTO.Name);
                throw;
            }
        }

        public async Task<bool> TagUpdateAsync(int tagId, TagUpdateDTO tagUpdateDTO)
        {
            try
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
                if (tag == null)
                    return false;

                tag.Name = tagUpdateDTO.Name ?? tag.Name;
                tag.ModifiedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TagUpdateAsync for TagId {TagId}", tagId);
                throw;
            }
        }

        public async Task<bool> TagDeleteAsync(int tagId)
        {
            try
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
                if (tag == null)
                    return false;

                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TagDeleteAsync for TagId {TagId}", tagId);
                throw;
            }
        }

        public async Task<Tag?> GetTagByIdAsync(int tagId)
        {
            try
            {
                return await _context.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTagByIdAsync for TagId {TagId}", tagId);
                throw;
            }
        }

        public async Task<List<Tag>> GetAllTagsAsync()
        {
            try
            {
                return await _context.Tags.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllTagsAsync");
                throw;
            }
        }

        public async Task<List<Tag>> GetAllTagsFilteredAsync(int pageNumber, int pageSize, TagFiltersDTO tagFiltersDTO)
        {
            try
            {
                var query = _context.Tags.AsQueryable();

                if (tagFiltersDTO != null)
                {
                    if (!string.IsNullOrEmpty(tagFiltersDTO.Name))
                        query = query.Where(t => t.Name.Contains(tagFiltersDTO.Name));
                }

                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllTagsFilteredAsync");
                throw;
            }
        }
    }
}