using System.Collections.Generic;
using System.Threading.Tasks;
using task_management.Models;
using task_management.IRepositories;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            var projects = await _unitOfWork.Repository<Project>().GetAllAsync();
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
            _unitOfWork.Repository<Project>().Update(project);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            var projects = await _unitOfWork.Repository<Project>().GetByIdAsync(id);
            await _unitOfWork.CompleteAsync();
            return projects;

        }

        public async Task<Project> GetDetailedProject(int projectId)
        {
            var teamMembers =  _unitOfWork.ProjectAssignmentRepository.GetTeamMembersByProject(projectId);
            var staffRoles = await _identityService.GetUsersWithRoles(teamMembers);
            var project = await _unitOfWork.ProjectRepository.GetDetailedProject(projectId);
            return project;
        }

        public async Task<IEnumerable<Project>> GetProjectsByUserId(string userId)
        {
            var projectIdList = _unitOfWork.ProjectAssignmentRepository.GetProjectsByUserId(userId);

            var projects = await Task.WhenAll(projectIdList
                .Select(async project => await _unitOfWork.Repository<Project>().GetByIdAsync(project.projectId)));
            await _unitOfWork.CompleteAsync();
            return projects;
        }

        public async Task AddUserToProject (int projectId, string userId)
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



    }
}
