using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using task_management.Models;
using task_management.Services;

namespace task_management.Controllers
{
    /*
    public class HomeController : Controller
    {
        private readonly TaskService _taskService;

        public HomeController(TaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<IActionResult> Index()
        {
            var totalTasks = await _taskService.GetTotalTasksAsync();
            var completedTasks = await _taskService.GetCompletedTasksAsync();
            var totalEmployees = await _taskService.GetTotalEmployeesAsync();
            var completionRate = await _taskService.GetCompletionRateAsync();

            var model = new DashboardViewModel
            {
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                CompletionRate = completionRate,
                TotalEmployees = totalEmployees
            };

            return View(model);
        }
        
    }
    */
}

