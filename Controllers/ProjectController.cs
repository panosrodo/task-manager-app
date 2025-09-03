using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.Core.Enums;
using TaskManagerApp.Core.Filters;
using TaskManagerApp.DTO;
using TaskManagerApp.Services;

namespace TaskManagerApp.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: /Project/Index?pageNumber=1&pageSize=10&title=abc&ownerId=1
        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string? title = null, int? ownerId = null)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            var filters = new ProjectFiltersDTO
            {
                Title = title,
                OwnerId = ownerId
            };

            var projects = await _projectService.GetAllProjectsFilteredAsync(pageNumber, pageSize, filters);
            return View(projects);
        }

        // GET: /Project/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            var project = await _projectService.GetProjectAsync(id);
            if (project == null)
                return NotFound();

            return View(project);
        }

        // GET: /Project/Create
        [HttpGet]
        public IActionResult Create()
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            return View();
        }

        // POST: /Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectCreateDTO model)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            // OwnerId = Admin who creates the project
            model.OwnerId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            var result = await _projectService.CreateProjectAsync(model);
            if (result == null)
            {
                ModelState.AddModelError("", "Απέτυχε η δημιουργία.");
                return View(model);
            }

            // After creation, redirect to the list
            return RedirectToAction("Index");
        }

        // GET: /Project/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            var project = await _projectService.GetProjectAsync(id);
            if (project == null)
                return NotFound();

            var model = new ProjectUpdateDTO
            {
                Name = project.Name,
                Description = project.Description,
                OwnerId = project.OwnerId
            };

            return View(model);
        }

        // POST: /Project/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectUpdateDTO model)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            var success = await _projectService.UpdateProjectAsync(id, model);
            if (!success)
            {
                ModelState.AddModelError("", "Απέτυχε η ενημέρωση.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        // GET: /Project/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            var project = await _projectService.GetProjectAsync(id);
            if (project == null)
                return NotFound();

            return View(project);
        }

        // POST: /Project/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            var success = await _projectService.DeleteProjectAsync(id);
            if (!success)
                return NotFound();

            return RedirectToAction("Index");
        }
    }
}