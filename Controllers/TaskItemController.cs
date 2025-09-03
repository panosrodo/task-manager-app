using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.Services;
using TaskManagerApp.DTO;
using TaskManagerApp.Core.Enums;

namespace TaskManagerApp.Controllers
{
    public class TaskItemController : Controller
    {
        private readonly ITaskItemService _taskItemService;

        public TaskItemController(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        // GET: /TaskItem/Index?projectId=1&assigneeId=2&status=Open&priority=3
        // Display all tasks (visible to everyone)
        [HttpGet]
        public async Task<IActionResult> Index(
            int? projectId = null,
            int? assigneeId = null,
            TaskItemStatus? status = null,
            int? priority = null
        )
        {
            List<TaskItemReadOnlyDTO> tasks;

            // Filter priority: project, assignee, status, priority
            if (projectId.HasValue)
                tasks = await _taskItemService.GetTaskItemsByProjectAsync(projectId.Value);
            else if (assigneeId.HasValue)
                tasks = await _taskItemService.GetTaskItemsByAssigneeAsync(assigneeId.Value);
            else if (status.HasValue)
                tasks = await _taskItemService.GetTaskItemsByStatusAsync(status.Value);
            else if (priority.HasValue)
                tasks = await _taskItemService.GetTaskItemsByPriorityAsync(priority.Value);
            else
                tasks = new List<TaskItemReadOnlyDTO>();

            return View(tasks);
        }

        // GET: /TaskItem/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var task = await _taskItemService.GetTaskItemAsync(id);
            if (task == null)
                return NotFound();

            return View(task); // Task is visible to all users
        }

        // GET: /TaskItem/Create
        [HttpGet]
        public IActionResult Create()
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            return View();
        }

        // POST: /TaskItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItemCreateDTO model)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            var result = await _taskItemService.CreateTaskItemAsync(model);
            if (result == null)
            {
                ModelState.AddModelError("", "Απέτυχε η δημιουργία.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        // GET: /TaskItem/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            var task = await _taskItemService.GetTaskItemAsync(id);
            if (task == null)
                return NotFound();

            var model = new TaskItemUpdateDTO
            {
                TaskItemId = task.TaskItemId,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status,
                ProjectId = task.ProjectId,
                AssigneeId = task.AssigneeId
            };

            return View(model);
        }

        // POST: /TaskItem/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskItemUpdateDTO model)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            model.TaskItemId = id;

            var success = await _taskItemService.UpdateTaskItemAsync(model);
            if (!success)
            {
                ModelState.AddModelError("", "Απέτυχε η ενημέρωση.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        // GET: /TaskItem/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            var task = await _taskItemService.GetTaskItemAsync(id);
            if (task == null)
                return NotFound();

            return View(task);
        }

        // POST: /TaskItem/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            var success = await _taskItemService.DeleteTaskItemAsync(id);
            if (!success)
                return NotFound();

            return RedirectToAction("Index");
        }

        // POST: /TaskItem/UpdateStatus/{id}
        // Allows a User to change only the status of the task
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, TaskItemStatus status)
        {
            // Action can be performed by either Admin or User
            if (!User.IsInRole(UserRole.User.ToString()) && !User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            // Only the status is updated!
            var model = new TaskItemUpdateDTO
            {
                TaskItemId = id,
                Status = status
            };

            var success = await _taskItemService.UpdateTaskItemAsync(model);
            if (!success)
            {
                ModelState.AddModelError("", "Απέτυχε η αλλαγή status.");
                // You can either redirect to the Details page or return the view
                return RedirectToAction("Details", new { id });
            }

            return RedirectToAction("Details", new { id });
        }
    }
}