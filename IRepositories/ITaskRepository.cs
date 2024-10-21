using task_management.Models;

namespace task_management.IRepositories
{
    public interface ITaskRepository : IRepository<Tasks>
    {
        public Task<Tasks> GetTaskById(int id);
        public IEnumerable<Tasks> GetTasksByUserId(string userId);
        void InActive(Tasks task);


    }
}
