using task_management.Models;

namespace task_management.IRepositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<List<Project>> GetProjectByUserId(string userId);

        Task<Project> GetDetailedProject(int projectId);
        void InActive(Project project);

    }
}
