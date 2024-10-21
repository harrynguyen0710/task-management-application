using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using task_management.IRepositories;
using task_management.Models;
using task_management.Services;
using task_management.ViewModels;

namespace task_management.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectService _projectService;
        private readonly UserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IdentityService _identityService;
        private readonly TaskService _taskService;

        public ProjectController(ProjectService projectService, UserService userService,
            IUnitOfWork unitOfWork, IdentityService identityService, TaskService taskService)
        {
            _projectService = projectService;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _identityService = identityService;
            _taskService = taskService;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return View(projects);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            await _projectService.AddProjectAsync(project);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project project)
        {
            await _projectService.UpdateProjectAsync(project);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id, int pageNumber = 1, int pageSize = 10)
        {
            var project = await _projectService.GetDetailedProject(id);
            if (project == null)
            {
                return NotFound();
            }

            // Fetch team members synchronously (non-async method)
            var teamMembers = _unitOfWork.ProjectAssignmentRepository.GetTeamMembersByProject(id);

            // Fetch available users asynchronously
            var availableUsers = await _userService.GetAvailableUsers(id);

            // Fetch the roles of the team members
            var staffRolesTask = _identityService.GetUsersWithRoles(teamMembers);

            // Fetch the tasks related to the project, with pagination
            var jointTasksTask = _taskService.GetTasksInProjects(teamMembers, pageNumber, pageSize);

            // Wait for both tasks (roles and tasks) to complete in parallel
            var staffRoles = await staffRolesTask;
            var jointTasks = await jointTasksTask;

            // Prepare the project details model
            var detailProject = new ProjectDetails()
            {
                Project = project,
                UserRole = (List<UserRoles>)staffRoles,
                Tasks = jointTasks
            };
            
            // Populate the ViewBag with available users for the dropdown selection
            ViewBag.AvailableUsers = new SelectList(availableUsers, "Id", "UserName");
            
            return View(detailProject);
        }


        [HttpPost]
        public async Task<IActionResult> AddUserToProject(int projectId, string userId)
        {
            await _projectService.AddUserToProject(projectId, userId);
            return RedirectToAction("Details", new { id = projectId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserToProject(int projectId, string userId)
        {
            await _projectService.RemoveUserToProject(projectId, userId);
            return RedirectToAction("Details", new { id = projectId });
        }

        [HttpPatch]
        public async Task<IActionResult> InActiveProject(Project project)
        {
            await _projectService.InActiveProjectAsync(project);
            return RedirectToAction("Details", new { id = project.projectId });
        }

    }
}
