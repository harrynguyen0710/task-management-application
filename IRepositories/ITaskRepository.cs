using task_management.Models;

namespace task_management.IRepositories
{
    public interface ITaskRepository : IRepository<Task>
    {
        public Task<Tasks> GetTaskById(int id);
        public IEnumerable<Tasks> GetTasksByUserId(string userId);
    }
}
