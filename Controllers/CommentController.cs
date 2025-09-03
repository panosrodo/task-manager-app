using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.Services;
using TaskManagerApp.DTO;

namespace TaskManagerApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // GET: /Comment/Index?taskItemId=123&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> Index(int? taskItemId = null, int pageNumber = 1, int pageSize = 10)
        {
            var filters = new TaskManagerApp.Core.Filters.CommentFiltersDTO
            {
                TaskId = taskItemId
            };

            var comments = await _commentService.GetCommentsFilteredAsync(filters, pageNumber, pageSize);
            return View(comments);
        }

        // GET: /Comment/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var comment = await _commentService.GetCommentAsync(id);
            if (comment == null)
                return NotFound();

            return View(comment); // Visible to all users
        }

        // GET: /Comment/Create?taskItemId=123
        [HttpGet]
        public IActionResult Create(int taskItemId)
        {
            var model = new CommentCreateDTO { TaskItemId = taskItemId };
            return View(model);
        }

        // POST: /Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentCreateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // UserId is retrieved from the authentication context.
            model.UserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            var result = await _commentService.CreateCommentAsync(model);
            if (result == null)
            {
                ModelState.AddModelError("", "Απέτυχε η δημιουργία.");
                return View(model);
            }

            return RedirectToAction("Index", new { taskItemId = model.TaskItemId });
        }

        // GET: /Comment/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var comment = await _commentService.GetCommentAsync(id);
            if (comment == null)
                return NotFound();

            // Checks if the current user is the owner of the comment
            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (comment.UserId != currentUserId)
                return Forbid();

            var model = new CommentUpdateDTO
            {
                CommentId = comment.CommentId,
                Text = comment.Text
            };

            return View(model);
        }

        // POST: /Comment/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CommentUpdateDTO model)
        {
            var comment = await _commentService.GetCommentAsync(id);
            if (comment == null)
                return NotFound();

            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (comment.UserId != currentUserId)
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            model.CommentId = id;
            var success = await _commentService.UpdateCommentAsync(model);
            if (!success)
            {
                ModelState.AddModelError("", "Απέτυχε η ενημέρωση.");
                return View(model);
            }

            return RedirectToAction("Index", new { taskItemId = comment.TaskItemId });
        }

        // GET: /Comment/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _commentService.GetCommentAsync(id);
            if (comment == null)
                return NotFound();

            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (comment.UserId != currentUserId)
                return Forbid();

            return View(comment);
        }

        // POST: /Comment/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _commentService.GetCommentAsync(id);
            if (comment == null)
                return NotFound();

            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (comment.UserId != currentUserId)
                return Forbid();

            var success = await _commentService.DeleteCommentAsync(id);
            if (!success)
                return NotFound();

            return RedirectToAction("Index", new { taskItemId = comment.TaskItemId });
        }
    }
}