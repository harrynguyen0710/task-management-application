using Microsoft.EntityFrameworkCore;
using task_management.Data;
using task_management.IRepositories;
using task_management.Models;

namespace task_management.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Project> GetDetailedProject(int id)
        {
            return await _context.Projects
                .Where(pr => pr.projectId == id)
                .Include(p => p.ProjectAssignments)
                .ThenInclude(u => u.User)
                .FirstOrDefaultAsync();

        }

        public async Task<List<Project>> GetProjectByUserId(string userId)
        {
            var projects = await _context.ProjectAssignments
                .Select(p => p.Project)
                .ToListAsync();
            return projects;
        }

        public void InActive(Project project)
        {
            project.isActive = false;
        }
    }
}
