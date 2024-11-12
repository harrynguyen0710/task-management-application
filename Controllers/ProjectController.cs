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
            Console.WriteLine("Hihi, I'm calling project details");

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
            Console.WriteLine("feel the pain");
            Console.WriteLine($"ProjectId: {projectId}, PageNumber: {pageNumber}, SearchTerm: {searchTerm}, Status: {status}, Priority: {priority}, Assignee: {assignee}");
            // Get team members associated with the project
            var teamMembers = _unitOfWork.ProjectAssignmentRepository.GetTeamMembersByProject(projectId);

            Console.WriteLine($"team members::{teamMembers}");

            // Retrieve all tasks associated with the team members in the project
            var allTasks = await _taskService.GetTasksInProjects(teamMembers, 1, int.MaxValue);

            Console.WriteLine($"all tasks::{allTasks}");

            // Filter tasks by search term
            var filteredTasks = string.IsNullOrEmpty(searchTerm)
                ? allTasks.ToList()
                : allTasks.Where(t => t.name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            Console.WriteLine($"filter tasks:: {filteredTasks}");

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
                filteredTasks = filteredTasks.Where(t => t.userId == assignee).ToList(); 
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
        public async Task<IActionResult> AddUserToProject(int projectId, string userId)
        {
            try
            {
                // Thêm người dùng vào dự án
                await _projectService.AddUserToProject(projectId, userId);

                // Lấy thông tin người dùng và vai trò sử dụng GetUsersWithRoles
                var projectAssignment = new List<ProjectAssignment> { new ProjectAssignment { userId = userId } };
                var usersWithRoles = await _identityService.GetUsersWithRoles(projectAssignment);

                // Kiểm tra xem người dùng có tồn tại không
                var addedUser = usersWithRoles.FirstOrDefault();
                if (addedUser == null)
                {
                    return Json(new { success = false, message = "User not found." });
                }

                // Chuẩn bị JSON trả về
                var userInfo = new
                {
                    id = addedUser.Id,
                    FullName = addedUser.fullName,
                    roles = addedUser.Roles.FirstOrDefault(), // Lấy vai trò đầu tiên nếu có nhiều vai trò
                    imageUrl = addedUser.ImageUrl
                };

                return Json(new { success = true, message = "User added to the project successfully!", user = userInfo });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to add user to the project.", error = ex.Message });
            }
        }

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
