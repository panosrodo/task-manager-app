using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerApp.Core.Enums;
using TaskManagerApp.Core.Filters;
using TaskManagerApp.DTO;
using TaskManagerApp.Services;

namespace TaskManagerApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /User/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginDTO model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userService.VerifyAndGetUserAsync(model);
            if (user == null)
            {
                ModelState.AddModelError("", "Λάθος username ή password.");
                return View(model);
            }

            // Set claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            // Redirect to dashboard
            return RedirectToAction("Dashboard", "Home");
        }

        // GET: /User/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserCreateDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userService.CreateUserAsync(model);
            if (user == null)
            {
                ModelState.AddModelError("", "Απέτυχε η εγγραφή. Δοκιμάστε ξανά.");
                return View(model);
            }

            // Login after registration
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Dashboard", "Home");
        }

        // GET: /User/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }

        // GET: /User/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            var users = await _userService.GetAllUsersFiltered(1, 100, new UserFiltersDTO());
            return View(users);
        }

        // GET: /User/Details/{username}
        [HttpGet]
        public async Task<IActionResult> Details(string username)
        {
            // Only admins can view other users; each user can only view their own profile
            if (!User.IsInRole(UserRole.Admin.ToString()) &&
                User.Identity?.Name != username)
                return Forbid();

            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // GET: /User/Edit/{username}
        [HttpGet]
        public async Task<IActionResult> Edit(string username)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
                return NotFound();

            var model = new UserUpdateDTO
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            ViewBag.Username = username;
            return View(model);
        }
        // POST: /User/Edit/{username}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string username, UserUpdateDTO model)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            var success = await _userService.UpdateUserAsync(username, model);
            if (!success)
            {
                ModelState.AddModelError("", "Απέτυχε η επεξεργασία.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        // GET: /User/Delete/{username}
        [HttpGet]
        public async Task<IActionResult> Delete(string username)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: /User/Delete/{username}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string username)
        {
            if (!User.IsInRole(UserRole.Admin.ToString()))
                return Forbid();

            var success = await _userService.DeleteUserAsync(username);
            if (!success)
                return NotFound();

            return RedirectToAction("Index");
        }
    }
}