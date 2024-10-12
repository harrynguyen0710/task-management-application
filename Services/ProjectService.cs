using task_management.IRepositories;
using task_management.Models;

namespace task_management.Services
{
    public class ProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            var projectRepository = _unitOfWork.Repository<Project>();
            return await projectRepository.GetAllAsync();
        }

        public async Task AddProjectAsync(Project project)
        {
            var projectRepository = _unitOfWork.Repository<Project>();
            projectRepository.Add(project);
            await _unitOfWork.CompleteAsync();
        }
    }
}
