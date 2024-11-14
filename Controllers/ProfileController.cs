using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using task_management.Models;
using task_management.Services;
using task_management.ViewModels;

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

        public ProfileController(ILogger<ProfileController> logger, UserService userService,
            UserManager<Users> userManager, IdentityService identityService, ProjectService projectService, TaskService taskService)
        {
            _logger = logger;
            _userService = userService;
            _userManager = userManager;
            _identityService = identityService;
            _projectService = projectService;
            _taskService = taskService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userRoles = await _identityService.GetUserRole(user);
            var detailedUser = await _userService.GetDetailUser(user.Id);
            detailedUser.RoleNames = userRoles;
            detailedUser.User = user;
            return View(detailedUser);
        }

        [HttpGet("Profile/GetTaskDetails/{taskId}")]
        public async Task<IActionResult> GetTaskDetails(int taskId)
        {
            var task = await _userService.GetTaskById(taskId);
            if (task == null)
            {
                return NotFound();
            }

            var userProjects = await _projectService.GetProjectsByUserId(task.userId);

            Project project = null;
            foreach (var userProject in userProjects)
            {
                var tasksInProject = await _taskService.GetTasksByProjectId(userProject.projectId);
                if (tasksInProject.Any(t => t.taskId == taskId))
                {
                    project = userProject;
                    break;
                }
            }

            if (project == null)
            {
                return NotFound();
            }

            var userDetails = new UserDetails
            {
                Task = task,
                Project = project,
                User = task.User
            };

            return PartialView("_UsersDetailTaskPartial", userDetails);
        }
    }
}
