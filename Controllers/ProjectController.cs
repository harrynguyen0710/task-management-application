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

        public async Task<IActionResult> Details(int id, int pageNumber = 1, int pageSize = 5, int selectedTaskId = 0)
        {
            var project = await _projectService.GetDetailedProject(id);
            if (project == null)
            {
                return NotFound();
            }

            var teamMembers = _unitOfWork.ProjectAssignmentRepository.GetTeamMembersByProject(id);
            var availableUsers = await _userService.GetAvailableUsers(id);
            var staffRoles = (List<UserRoles>)(await _identityService.GetUsersWithRoles(teamMembers));

            // Fetch the total number of tasks
            var totalTasks = _taskService.GetTotalTasks(id);

            // Fetch the paginated tasks
            var jointTasks = await _taskService.GetTasksInProjects(teamMembers, pageNumber, pageSize);

            var selectedTask = jointTasks.FirstOrDefault(t => t.taskId == selectedTaskId);

            var detailProject = new ProjectDetails()
            {
                Project = project,
                UserRole = staffRoles,
                Tasks = jointTasks,
                SelectedTask = selectedTask,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalTasks = totalTasks,
            };

            ViewBag.AvailableUsers = new SelectList(availableUsers, "Id", "fullName");
            ViewBag.Staff = new SelectList(staffRoles, "Id", "fullName");
            return View(detailProject);
        }


        public async Task<IActionResult> LoadTasks(int projectId, int pageNumber = 1, int pageSize = 5, string searchTerm = "", string status = "", string priority = "", string assignee = "")
        {
            Console.WriteLine($"ProjectId: {projectId}, PageNumber: {pageNumber}, SearchTerm: {searchTerm}, Status: {status}, Priority: {priority}, Assignee: {assignee}");
            // Get team members associated with the project
            var teamMembers = _unitOfWork.ProjectAssignmentRepository.GetTeamMembersByProject(projectId);

            // Retrieve all tasks associated with the team members in the project
            var allTasks = await _taskService.GetTasksInProjects(teamMembers, 1, int.MaxValue);

            // Filter tasks by search term
            var filteredTasks = string.IsNullOrEmpty(searchTerm)
                ? allTasks.ToList()
                : allTasks.Where(t => t.name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            // Apply additional filters if provided
            if (!string.IsNullOrEmpty(status))
            {
                filteredTasks = filteredTasks.Where(t => t.status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(priority))
            {
                filteredTasks = filteredTasks.Where(t => t.priority.Equals(priority, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(assignee))
            {
                filteredTasks = filteredTasks.Where(t => t.userId == assignee).ToList(); // Adjust based on your model
            }

            // Count of filtered tasks
            var filteredTasksCount = filteredTasks.Count;

            // Paginate the filtered tasks
            var paginatedTasks = filteredTasks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Prepare the ProjectDetails view model
            var detailProject = new ProjectDetails
            {
                Project = await _projectService.GetDetailedProject(projectId),
                UserRole = (List<UserRoles>)(await _identityService.GetUsersWithRoles(teamMembers)),
                Tasks = paginatedTasks,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalTasks = filteredTasksCount
            };

            // Return the partial view with the task details
            return PartialView("_TasksPartial", detailProject);
        }





        public async Task<IActionResult> FullMemberList(
            int projectId,
            int pageNumber = 1,
            int pageSize = 5,
            string searchTerm = "",
            string status = "",
            string priority = "",
            string assignee = ""
        )
        {

            var project = await _projectService.GetDetailedProject(projectId);
            if (project == null)
            {
                return NotFound();
            }

            // Fetch team members and their additional details
            var teamMembers = _unitOfWork.ProjectAssignmentRepository.GetTeamMembersByProject(projectId);
            var staffRoles = (List<UserRoles>)(await _identityService.GetUsersWithRoles(teamMembers));

            var detailedMembers = new List<UserMembers>();
            foreach (var member in teamMembers)
            {
                var user = await _userService.GetUserById(member.userId);
                if (user != null)
                {
                    detailedMembers.Add(new UserMembers
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        ImageUrl = user.ImageUrl,
                        Roles = user.Roles
                    });
                }
            }

            // Retrieve all tasks associated with the team members in the project
            var allTasks = await _taskService.GetTasksInProjects(teamMembers, 1, int.MaxValue);

            // Filter tasks by search term
            var filteredTasks = string.IsNullOrEmpty(searchTerm)
                ? allTasks.ToList()
                : allTasks.Where(t => t.name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            // Apply additional filters if provided
            if (!string.IsNullOrEmpty(status))
            {
                filteredTasks = filteredTasks.Where(t => t.status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(priority))
            {
                filteredTasks = filteredTasks.Where(t => t.priority.Equals(priority, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(assignee))
            {
                filteredTasks = filteredTasks.Where(t => t.userId == assignee).ToList(); // Adjust based on your model
            }

            // Count of filtered tasks
            var filteredTasksCount = filteredTasks.Count;

            // Paginate the filtered tasks
            var paginatedTasks = filteredTasks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Prepare the ProjectDetails view model with filtered and paginated tasks
            var detailProject = new ProjectDetails
            {
                Project = project,
                UserRole = staffRoles,
                Tasks = paginatedTasks,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalTasks = filteredTasksCount,
            };

            return View("MemberList", detailProject);

        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserToProject(int projectId, string userId)
        {
            await _projectService.RemoveUserToProject(projectId, userId);
            TempData["RemoveUserMessage"] = "User removed from the project successfully.";
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
