using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task_management.Data;
using task_management.Models;
using task_management.Services;

namespace task_management.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly UserService _userService;
        private readonly IdentityService _identityService;
        private readonly UserManager<Users> _userManager;
        private readonly ProjectService _projectService;
        private readonly TaskService _taskService;
        private readonly ApplicationDbContext _context;

        public ProfileController(ILogger<ProfileController> logger, UserService userService,
            UserManager<Users> userManager, IdentityService identityService, ProjectService projectService,
            TaskService taskService, ApplicationDbContext context)

        {
            _logger = logger;
            _userService = userService;
            _userManager = userManager;
            _identityService = identityService;
            _projectService = projectService;
            _taskService = taskService;
            _context = context;

        }

        public async Task<IActionResult> Index(string userId, int pageNumber = 1, int pageSize = 6)
        {
            var user = userId == null
                ? await _userManager.GetUserAsync(User)
                : await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            var userRoles = await _identityService.GetUserRole(user);
            var detailedUser = await _userService.GetDetailUser(user.Id);

            // Get tasks for the first page
            var tasks = await _taskService.GetTasksByUserId(userId, pageNumber, pageSize);
            detailedUser.RoleNames = userRoles;
            detailedUser.User = user;
            detailedUser.Tasks = tasks;

            double taskCounts = await _taskService.GetTotalUserTask(user.Id);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling(taskCounts / pageSize);

            return View(detailedUser);
        }


        public async Task<IActionResult> GetTasksPartial(string userId, int pageNumber = 1, int pageSize = 6)
        {
            var tasks = await _taskService.GetTasksByUserId(userId, pageNumber, pageSize);
            double taskCounts = await _taskService.GetTotalUserTask(userId);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling(taskCounts / pageSize);

            return PartialView("_TasksTablePartial", tasks);
        }


        public async Task<IActionResult> GetTaskDetails(int taskId)
        {
            var task = await _taskService.GetTaskByIdAsync(taskId);

            if (task == null)
            {
                return NotFound();
            }

            return PartialView("_UsersDetailTaskPartial", task);
        }


    }
}
