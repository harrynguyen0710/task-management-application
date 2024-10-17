
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using task_management.Data;
using task_management.IRepositories;
using task_management.Models;
using System.Collections.Generic;
using System.Linq;
using task_management.Enum;

namespace task_management.Repositories
{
    public class TaskRepository : Repository<Task>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> GetTotalTasksAsync()
        {
            return await _context.Tasks.CountAsync();
        }
        /*
        public async Task<int> GetTotalEmployeesAsync()
        {
            return await _context.Set<Users>().CountAsync();
        }
        */
        public async Task<int> FilterTask(string status)
        {
            return await _context.Tasks.CountAsync(t => t.status.ToString()  == status);
        }

        public Task<int> GetTotalEmployeesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
