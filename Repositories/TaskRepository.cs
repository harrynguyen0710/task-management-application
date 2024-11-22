using Microsoft.EntityFrameworkCore;
using task_management.Data;
using task_management.IRepositories;
using task_management.Models;

namespace task_management.Repositories
{
    public class TaskRepository : Repository<Tasks>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Tasks> GetTaskById(int id)
        {
            return await _context.Tasks
                .Include(u => u.ProjectAssignment)
                .ThenInclude(pa => pa.Project)
                .FirstOrDefaultAsync(t => t.taskId == id);
        }

        public async Task<IEnumerable<Tasks>> GetTasksByProjectId(
            int projectId,
            int pageNumber = 1,
             int pageSize = 8,
            string? status = null,
            string? priority = null,
            string? assignee = null)
        {
            var query = _context.Tasks
                    .Include(u => u.ProjectAssignment)
                    .ThenInclude(us => us.User)
                    .Where(t => t.projectId == projectId && t.isActive);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(t => t.status.Equals(status));

            if (!string.IsNullOrEmpty(priority))
                query = query.Where(t => t.priority.Equals(priority));

            if (!string.IsNullOrEmpty(assignee))
                query = query.Where(t => t.userId == assignee);

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public List<Tasks> GetTasksByUserId(string userId)
        {
            return _context.Tasks
                .Where(u => u.userId == userId && u.isActive)
                .Include(u => u.ProjectAssignment)
                .ThenInclude(pa => pa.Project)
                .OrderByDescending(t => t.startDate)
                .ToList();
        }

        public async Task<double> GetTotalUserTask(string userId)
        {
            return await _context.Tasks
                    .Where(t => t.isActive)
                    .CountAsync(t => t.userId == userId);
        }

        public int GetTotalTaskInProject(int projectId)
        {
            return  _context.Tasks
                .Where(t => t.isActive)
                .Count(t => t.projectId == projectId);
        }


        public async Task<List<Tasks>> SearchTasksByName(string taskName)
        {
            return await _context.Tasks
                .Where(t => t.name.Contains(taskName) && t.isActive)
                .ToListAsync();
        }


        public void InActive(Tasks task)
        {
            task.isActive = false;
        }

        public async Task<List<Tasks>> GetTasksByUserId(string userId, int pageNumber = 1, int pageSize = 6)
        {
            return await _context.Tasks
                .Where(u => u.userId == userId && u.isActive)
                .Include(u => u.ProjectAssignment)
                .ThenInclude(pa => pa.Project)
                .OrderByDescending(t => t.startDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
