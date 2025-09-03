using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.Services;
using TaskManagerApp.DTO;
using TaskManagerApp.Core.Enums;

namespace TaskManagerApp.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // GET: /Tag/Index?pageNumber=1&pageSize=10&name=searchText
        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string? name = null)
        {
            var filters = new TaskManagerApp.Core.Filters.TagFiltersDTO
            {
                Name = name
            };

            var tags = await _tagService.GetAllTagsFilteredAsync(pageNumber, pageSize, filters);
            return View(tags);
        }

        // GET: /Tag/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null)
                return NotFound();

            return View(tag); // Visible to all users
        }

        // GET: /Tag/Create
        [HttpGet]
        public IActionResult Create()
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            return View();
        }

        // POST: /Tag/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagCreateDTO model)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            var result = await _tagService.TagCreateAsync(model);
            if (result == null)
            {
                ModelState.AddModelError("", "Απέτυχε η δημιουργία.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        // GET: /Tag/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null)
                return NotFound();

            var model = new TagUpdateDTO
            {
                TagId = tag.Id,
                Name = tag.Name
            };

            return View(model);
        }

        // POST: /Tag/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TagUpdateDTO model)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            var success = await _tagService.TagUpdateAsync(id, model);
            if (!success)
            {
                ModelState.AddModelError("", "Απέτυχε η ενημέρωση.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        // GET: /Tag/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null)
                return NotFound();

            return View(tag);
        }

        // POST: /Tag/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            var success = await _tagService.TagDeleteAsync(id);
            if (!success)
                return NotFound();

            return RedirectToAction("Index");
        }
    }
}