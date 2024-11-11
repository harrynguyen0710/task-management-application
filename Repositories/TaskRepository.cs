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
            return await _context.Tasks.Include(u => u.User).Where(t => t.taskId == id).FirstOrDefaultAsync();  
        }

        public  IEnumerable<Tasks> GetTasksByUserId(string userId)
        {
            return  _context.Tasks.Where(u => u.userId == userId).ToList();
        }

        public int GetTotalTask(string userId)
        {
            return _context.Tasks.Where(u => u.userId == userId).Count();
        }

        //public async Task<int> GetTotalTask(int projectId)
        //{
        //    return _context.Tasks.Where(t => t.pr)
        //}

        public void InActive(Tasks task)
        {
            task.isActive = false;
        }
    }
}
