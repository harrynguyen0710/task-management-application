using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using task_management.Models;
using task_management.Services;  

namespace task_management.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectService _projectService;

        // Inject the ProjectService via constructor
        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        // List all projects
        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return View(projects);
        }

        // Get project details by ID
        public async Task<IActionResult> Details(int id)
        {
            var projects = await _projectService.GetAllProjectsAsync();
            var project = projects.FirstOrDefault(s => s.projectId == id);  
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // Example: Create a new project (GET method for form)
        public IActionResult Create()
        {
            return View();
        }

        // Example: Create a new project (POST method to save)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {

            await _projectService.AddProjectAsync(project);
            return RedirectToAction(nameof(Index));
            
/*            return View(student);
*/        }

    }
}
