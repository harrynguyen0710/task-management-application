using task_management.IRepositories;
using task_management.Models;

namespace task_management.Services
{
    public class ProjectAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectAssignmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectAssignment> GetById(int assignId)
        {
            var assignment = await _unitOfWork.Repository<ProjectAssignment>().GetByIdAsync(assignId);
            return assignment;
        }

        public List<Project> GetProjectsByUserId(string userId)
        {
            var projects =  _unitOfWork.ProjectAssignmentRepository.GetProjectsByUserId(userId); 
            return projects;
        }
        public async Task UpdateAssignment(ProjectAssignment projectAssignment)
        {
            _unitOfWork.Repository<ProjectAssignment>().Update(projectAssignment);
            await _unitOfWork.CompleteAsync();
        }
    }
}
