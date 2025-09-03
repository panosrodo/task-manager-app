using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.Services;
using TaskManagerApp.DTO;
using TaskManagerApp.Core.Enums;

namespace TaskManagerApp.Controllers
{
    public class TaskTagController : Controller
    {
        private readonly ITaskTagService _taskTagService;
        private readonly ITaskItemService _taskItemService;

        public TaskTagController(ITaskTagService taskTagService, ITaskItemService taskItemService)
        {
            _taskTagService = taskTagService;
            _taskItemService = taskItemService;
        }

        // GET: /TaskTag/Index?taskItemId=1&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> Index(int? taskItemId = null, int? tagId = null, int pageNumber = 1, int pageSize = 10)
        {
            var taskTags = await _taskTagService.GetAllTaskTagsFilteredAsync(pageNumber, pageSize, taskItemId, tagId);
            return View(taskTags);
        }

        // POST: /TaskTag/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskTagCreateDTO model)
        {
            // Retrieve the task to check permissions
            var task = await _taskItemService.GetTaskItemAsync(model.TaskItemId);

            if (task == null)
                return NotFound();

            // If the user is not an admin and not the assignee of the task, forbid the action
            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var isAdmin = User.IsInRole(UserRole.Admin.ToString());
            var isAssignee = (task.AssigneeId == currentUserId);

            if (!isAdmin && !isAssignee)
                return Forbid();

            var entity = new TaskManagerApp.Data.TaskTag
            {
                TaskItemId = model.TaskItemId,
                TagId = model.TagId
            };

            await _taskTagService.CreateAsync(entity);

            return RedirectToAction("Index", new { taskItemId = model.TaskItemId });
        }

        // POST: /TaskTag/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int taskItemId, int tagId)
        {
            // If the user is not an admin and not the assignee of the task, forbid the action
            var task = await _taskItemService.GetTaskItemAsync(taskItemId);

            if (task == null)
                return NotFound();

            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var isAdmin = User.IsInRole(UserRole.Admin.ToString());
            var isAssignee = (task.AssigneeId == currentUserId);

            if (!isAdmin && !isAssignee)
                return Forbid();

            await _taskTagService.DeleteAsync(taskItemId, tagId);

            return RedirectToAction("Index", new { taskItemId });
        }
    }
}