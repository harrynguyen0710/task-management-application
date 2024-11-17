using task_management.Models;

namespace task_management.IRepositories
{
    public interface ITaskRepository : IRepository<Tasks>
    {
        public Task<Tasks> GetTaskById(int id);
        public Task<double> GetTotalUserTask(string userId);
        public Task<IEnumerable<Tasks>> GetTasksByProjectId(int projectId, int pageNumber = 1, int pageSize = 8, string? status = null, string? priority = null, string? assignee = null);

        public int GetTotalTaskInProject(int projectId);
        public List<Tasks> GetTasksByUserId(string userId);
        public Task<List<Tasks>> GetTasksByUserId(string userId, int pageNumber = 1, int pageSize = 6);

        public Task<List<Tasks>> SearchTasksByName(string taskName);

        void InActive(Tasks task);

    }
}
