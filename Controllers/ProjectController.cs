using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using task_management.IRepositories;
using task_management.Models;
using task_management.Services;
using task_management.ViewModels;

namespace task_management.Controllers
{

    [Authorize]
    public class ProjectController : Controller
    {
        const string MANAGER = "Manager";
        const string STAFF = "Staff";

        private readonly ProjectService _projectService;
        private readonly UserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IdentityService _identityService;
        private readonly TaskService _taskService;
        private readonly ProjectAssignmentService _projectAssignmentService;
        private readonly UserManager<Users> _userManager;

        public ProjectController(ProjectService projectService, UserService userService,
            IUnitOfWork unitOfWork, IdentityService identityService,
            TaskService taskService, ProjectAssignmentService projectAssignmentService, UserManager<Users> userManager)
        {
            _projectService = projectService;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _identityService = identityService;
            _taskService = taskService;
            _projectAssignmentService = projectAssignmentService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var role = await _userManager.GetRolesAsync(user);

            var roleName = role.FirstOrDefault();
            var projects = roleName == MANAGER
            ? await _projectService.GetAllProjectsAsync()
            : _projectAssignmentService.GetProjectsByUserId(user.Id);

            return View(projects);

        }

        [Authorize(Roles = MANAGER)]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = MANAGER)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            project.isActive = true;
            await _projectService.AddProjectAsync(project);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = MANAGER)]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [Authorize(Roles = MANAGER)]
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
            var jointTasks = await _taskService.GetTasksInProjects(id, pageNumber, pageSize);

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
            // Get team members associated with the project
            var teamMembers = _unitOfWork.ProjectAssignmentRepository.GetTeamMembersByProject(projectId);

            // Retrieve all tasks associated with the team members in the project
            var allTasks = await _taskService.GetTasksInProjects(projectId, pageNumber, pageSize, status, priority, assignee);

            // Filter tasks by search term
            var filteredTasks = string.IsNullOrEmpty(searchTerm)
                ? allTasks
                : await _taskService.SearchTaskByName(searchTerm);
            
            var totalTasks = _taskService.GetTotalTasks(projectId);

            // Prepare the ProjectDetails view model
            var detailProject = new ProjectDetails
            {
                Project = await _projectService.GetDetailedProject(projectId),
                UserRole = (List<UserRoles>)(await _identityService.GetUsersWithRoles(teamMembers)),
                Tasks = filteredTasks,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalTasks = totalTasks
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
            var teamMembers = _unitOfWork.ProjectAssignmentRepository.GetTeamMembersByProject(projectId);
            
            ViewBag.ProjectId = projectId;

            ViewBag.Staff = new SelectList(teamMembers, "Id", "fullName");

           
            //// Fetch the paginated tasks
            var jointTasks = await _taskService.GetTasksInProjects(projectId, pageNumber, pageSize);

            // Prepare the ProjectDetails view model
            var detailProject = new ProjectDetails
            {
                UserRole = (List<UserRoles>)(await _identityService.GetUsersWithRoles(teamMembers)),
                PageNumber = pageNumber,
                PageSize = pageSize,
                Tasks = jointTasks,

            };

            return View("MemberList", detailProject);

        }

        [Authorize(Roles = MANAGER)]
        [HttpPost]
        public async Task<IActionResult> AddUserToProject(int projectId, string userId)
        {
            try
            {
                // Thêm người dùng vào dự án
                await _projectService.AddUserToProject(projectId, userId);


                // Lấy thông tin người dùng và vai trò sử dụng GetUsersWithRoles
                var usersWithRoles = await _userService.GetUserById(userId);   

                return Json(new { success = true, message = "User added to the project successfully!", user = usersWithRoles });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to add user to the project.", error = ex.Message });
            }
        }

        [Authorize(Roles = MANAGER)]
        [HttpPost]
        public async Task<IActionResult> RemoveUserFromProject(int projectId, string userId)
        {
            try
            {
                bool isRemoved = await _userService.RemoveUserFromProject(projectId, userId);

                if (!isRemoved)
                {
                    return Json(new { success = false, message = "Failed to remove user from the project. User not found or already removed." });
                }

                return Json(new { success = true, message = "User removed from the project successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to remove user from the project.", error = ex.Message });
            }
        }

        [Authorize(Roles = MANAGER)]
        [HttpPost]
        public async Task<IActionResult> RemoveUserToProject(int projectId, string userId)
        {
            await _projectService.RemoveUserToProject(projectId, userId);
            TempData["RemoveUserMessage"] = "User removed from the project successfully.";
            return RedirectToAction("Details", new { id = projectId });
        }

        [Authorize(Roles = MANAGER)]
        [HttpPatch]
        public async Task<IActionResult> InActiveProject(Project project)
        {
            await _projectService.InActiveProjectAsync(project);
            return RedirectToAction("Details", new { id = project.projectId });
        }

        [Authorize(Roles = MANAGER)]
        [HttpPut]
        public async Task<IActionResult> Remove(int projectId)
        {
            var project = await _projectService.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                return Json(new { success = false, message = "Project not found" });
            }

            await _projectService.InActiveProjectAsync(project);

            return Json(new { success = true, message = "Project deleted successfully" });
        }

    }
}
