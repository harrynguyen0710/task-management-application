using task_management.Models;

namespace task_management.IRepositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<Project> GetDetailedProject(int projectId);
        void InActive(Project project);

    }
}
