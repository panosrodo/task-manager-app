using TaskManagerApp.Core.Filters;
using TaskManagerApp.Data;
using TaskManagerApp.DTO;

namespace TaskManagerApp.Services
{
    public interface ITagService
    {
        Task<Tag?> TagCreateAsync(TagCreateDTO tagCreateDTO);
        Task<bool> TagUpdateAsync(int tagId, TagUpdateDTO tagUpdateDTO);
        Task<bool> TagDeleteAsync(int tagId);
        Task<Tag?> GetTagByIdAsync(int tagId);
        Task<List<Tag>> GetAllTagsAsync();
        Task<List<Tag>> GetAllTagsFilteredAsync(int pageNumber, int pageSize, TagFiltersDTO tagFiltersDTO);
    }
}
