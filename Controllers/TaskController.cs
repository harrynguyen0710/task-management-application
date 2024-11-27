using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using task_management.Models;
using task_management.Services;
using task_management.ViewModels;

namespace task_management.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly TaskService _taskService;
        private readonly UserService _userService;
        private readonly ProjectAssignmentService _projectAssignmentService;
        public TaskController(TaskService taskService, UserService userService, ProjectAssignmentService projectAssignmentService)
        {
            _taskService = taskService;
            _userService = userService;
            _projectAssignmentService = projectAssignmentService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddTask(int projectId)
        {
            var availableUsers = await _userService.GetUsersByProjectId(projectId);

            ViewBag.AvailableUsers = new SelectList(availableUsers, "Id", "fullName");
            var taskDetails = new TaskDetails
            {
                projectId = projectId,
            };

            return View(taskDetails);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskDetails taskDetails)
        {
            taskDetails.Tasks.projectId = taskDetails.projectId;
            await _taskService.AddTaskAsync(taskDetails.Tasks);
            TempData["AddTaskMessage"] = "Task added successfully!";
            return RedirectToAction("Details", "Project", new { id = taskDetails.projectId });
        }

        [HttpPatch]
        public async Task<IActionResult> InActiveTask(TaskDetails taskDetails)
        {
            await _taskService.InActiveTaskAsync(taskDetails.Tasks);
            TempData["InActiveTaskMessage"] = "Task removed from the project successfully!";
            return RedirectToAction("Details", "Project", new { id = taskDetails.projectId });
        }


        public async Task<IActionResult> Update(int projectId, int taskId)
        {
            var availableUsers = await _userService.GetUsersByProjectId(projectId);
            var task = await _taskService.GetTaskByIdAsync(taskId); 
            ViewBag.AvailableUsers = new SelectList(availableUsers, "Id", "fullName");
            var taskDetails = new TaskDetails
            {
                Tasks = task,
                projectId = projectId,
            };

            return View(taskDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TaskDetails taskDetails)
        {

            await _taskService.UpdateTaskAsync(taskDetails.Tasks);
            TempData["InActiveTaskMessage"] = "Task updated from the project successfully!";
            return RedirectToAction("Details", "Project", new { id = taskDetails.Tasks.projectId });
        }


        [HttpPut]
        public async Task<IActionResult> Remove(int projectId, int taskId)
        {
            var task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                return Json(new { success = false, message = "Task not found" });
            }

            await _taskService.InActiveTaskAsync(task);

            return Json(new { success = true, message = "Task deleted successfully" });
        }

    }
}
