using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using task_management.Services;
using task_management.ViewModels;

namespace task_management.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskService _taskService;
        private readonly UserService _userService;
        public TaskController(TaskService taskService, UserService userService)
        {
            _taskService = taskService;
            _userService = userService;
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


        /* public async Task<IActionResult> UpdateTask(int projectId)
         {
             var availableUsers = await _userService.GetUsersByProjectId(projectId);
             ViewBag.AvailableUsers = new SelectList(availableUsers, "Id", "UserName");
             var taskDetails = new TaskDetails
             {
                 projectId = projectId,
             };

             return View(taskDetails);
         }


         [HttpPost]
         public async Task<IActionResult> UpdateTask()
         {
             return RedirectToAction("Details", "Project", new { id = taskDetails.projectId });
         }*/

    }
}
