using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // Example: Create a new project (GET method for form)
        public async Task<IActionResult> Create()
        {
            // Get the organization list and create a SelectList
            ViewBag.Organizations = await _projectService.GetOrganizationsSelectListAsync();
            return View();
        }

        // POST method to create new project
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            await _projectService.AddProjectAsync(project);
            return RedirectToAction(nameof(Index));
        }
        // GET: Project/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            ViewBag.Organizations = await _projectService.GetOrganizationsSelectListAsync();
            return View(project);
        }

        // POST: Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project project)
        {
                await _projectService.UpdateProjectAsync(project);
                return RedirectToAction(nameof(Index));
        }
        // GET: Project/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectService.GetProjectDetailsAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }
    }
}
