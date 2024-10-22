using task_management.Models;

namespace task_management.IRepositories
{
    public interface ITaskRepository : IRepository<Tasks>
    {
        public Task<Tasks> GetTaskById(int id);
        public IEnumerable<Tasks> GetTasksByUserId(string userId);
        public int GetTotalTask(string userId);
        //public Task<int> GetTotalTask(int projectId);
        void InActive(Tasks task);


    }
}
