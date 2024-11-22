using task_management.IRepositories;
using task_management.Models;

namespace task_management.Services
{
    public class ProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IdentityService _identityService;

        public ProjectService(IUnitOfWork unitOfWork, IdentityService identityService)
        {
            _unitOfWork = unitOfWork;
            _identityService = identityService; 
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            var projects = await  _unitOfWork.ProjectRepository.GetAllAsync();
            return projects;
        }


        public async Task AddProjectAsync(Project project)
        {
            project.organizationId = 1;
            _unitOfWork.Repository<Project>().Add(project);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateProjectAsync(Project project)
        {
            project.organizationId = 1;
            _unitOfWork.Repository<Project>().Update(project);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            var projects = await _unitOfWork.Repository<Project>().GetByIdAsync(id);
            return projects;

        }

        public async Task<Project> GetDetailedProject(int projectId)
        {
            var project = await _unitOfWork.ProjectRepository.GetDetailedProject(projectId);
            return project;
        }

        public async Task<IEnumerable<Project>> GetProjectsByUserId(string userId)
        {
            var projects = await _unitOfWork.ProjectRepository.GetProjectByUserId(userId);
            return projects;
        }

        public async Task AddUserToProject(int projectId, string userId)
        {
            var newAssignment = new ProjectAssignment
            {
                projectId = projectId,
                userId = userId
            };
            _unitOfWork.Repository<ProjectAssignment>().Add(newAssignment);
            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveUserToProject(int projectId, string userId)
        {
            var assignment = new ProjectAssignment() { projectId = projectId, userId = userId };
            _unitOfWork.Repository<ProjectAssignment>().Remove(assignment);
            await _unitOfWork.CompleteAsync();
        }

        public async Task InActiveProjectAsync(Project project)
        {
            _unitOfWork.ProjectRepository.InActive(project);
            await _unitOfWork.CompleteAsync();
        }


    }
}
