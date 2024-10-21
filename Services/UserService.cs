using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using task_management.IRepositories;
using task_management.Models;

namespace task_management.Services
{
    public class UserService
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            return await _unitOfWork.Repository<Users>().GetAllAsync();

        }


        public async Task<IEnumerable<Users>> GetUsersByProjectId(int projectId)
        {
            var projectAssignments =  _unitOfWork.ProjectAssignmentRepository.GetTeamMembersByProject(projectId);

            var teamMembers = await Task.WhenAll(projectAssignments
                .Select(async assignment => await _userManager.FindByIdAsync(assignment.userId))
                .Where(user => user != null));

            await _unitOfWork.CompleteAsync();
            
            return teamMembers;
        }


        public async Task<IEnumerable<Users>> GetAvailableUsers(int projectId)
        {
            // Get all users and users assigned to the project
            var users = await GetAllUsersAsync();
            var projectUsers = await GetUsersByProjectId(projectId);

            // Filter users who are not part of the project
            var availableUsers = users.Where(u => !projectUsers.Any(pu => pu.Id == u.Id)).ToList();

            return availableUsers;
        }

    }
}
