using task_management.Models;

namespace task_management.IRepositories
{
    public interface ITaskRepository : IRepository<Tasks>
    {
        public Task<Tasks> GetTaskById(int id);

        public Task<double> GetTotalUserTask(string userId);

        public List<Tasks> GetTasksByUserId(string userId);
        public Task<List<Tasks>> GetTasksByUserId(string userId, int pageNumber = 1, int pageSize = 6);
        public int GetTotalTask(string userId);
        //public Task<int> GetTotalTask(int projectId);
        void InActive(Tasks task);


    }
}
