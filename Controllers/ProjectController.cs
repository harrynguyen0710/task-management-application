using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
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

        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectService.GetDetailedProject(id);
            if (project == null)
            {
                return NotFound();
            }
            // get all joint staff of the project.
            var teamMembers = _unitOfWork.ProjectAssignmentRepository.GetTeamMembersByProject(id);

            // get role names of them.
            var staffRoles = await _identityService.GetUsersWithRoles(teamMembers);

            // get involved tasks.
            var jointTasks = await _taskService.GetTasksInProjects(teamMembers);

            var detailProject = new ProjectDetails()
            {
                Project = project,
                UserRole = staffRoles,
                Tasks = jointTasks
            };

            // Filter users who are not part of the project.
            var availableUsers = await _userService.GetAvailableUsers(id);
            
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
    }
}
