using task_management.Models;

namespace task_management.IRepositories
{
    public interface ITaskRepository : IRepository<Task>
    {
        Task<int> GetTotalTasksAsync();
        Task<int> FilterTask(string status);
        Task<int> GetTotalEmployeesAsync();
    }
}
