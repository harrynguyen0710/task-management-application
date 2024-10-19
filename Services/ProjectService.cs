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

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            var projectRepository = _unitOfWork.Repository<Project>();
            var organizationRepository = _unitOfWork.Repository<Organization>();

            // Get all projects
            var projects = await projectRepository.GetAllAsync();

            // Get all organizations
            var organizations = await organizationRepository.GetAllAsync();

            // Assign organization name to each project
            foreach (var project in projects)
            {
                var organization = organizations.FirstOrDefault(o => o.organizationId == project.organizationId);
                project.Organization = organization; // Assign organization to project
            }

            return projects;
        }

        public async Task<SelectList> GetOrganizationsSelectListAsync()
        {
            var organizationRepository = _unitOfWork.Repository<Organization>();
            var organizations = await organizationRepository.GetAllAsync();
            return new SelectList(organizations, "organizationId", "name");
        }

        public async Task AddProjectAsync(Project project)
        {
            var projectRepository = _unitOfWork.Repository<Project>();
            projectRepository.Add(project);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateProjectAsync(Project project)
        {
            var projectRepository = _unitOfWork.Repository<Project>();
            projectRepository.Update(project);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            var projectRepository = _unitOfWork.Repository<Project>();
            return await projectRepository.GetByIdAsync(id);
        }
        public async Task<Project> GetProjectDetailsAsync(int id)
        {
            var projectRepository = _unitOfWork.Repository<Project>();
            var organizationRepository = _unitOfWork.Repository<Organization>();

            var project = await projectRepository.GetByIdAsync(id);
            if (project != null)
            {
                
                var organizations = await organizationRepository.GetAllAsync();
                var organization = organizations.FirstOrDefault(o => o.organizationId == project.organizationId);
                project.Organization = organization; 
            }
            return project;
        }
    }
}
