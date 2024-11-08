using Microsoft.AspNetCore.Identity;
using task_management.IRepositories;
using task_management.Models;
using task_management.ViewModels;

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
            var projectAssignments = _unitOfWork.ProjectAssignmentRepository.GetTeamMembersByProject(projectId);

            var teamMembers = await Task.WhenAll(projectAssignments
                .Select(async assignment => await _userManager.FindByIdAsync(assignment.userId))
                .Where(user => user != null));

            await _unitOfWork.CompleteAsync();

            return teamMembers;
        }

        public bool IsEmailExisted(string email)
        {
            return _unitOfWork.UserRepository.IsEmailExisted(email);
        }

        public bool IsPhoneNumberExisted(string phoneNumber)
        {
            return _unitOfWork.UserRepository.IsPhoneNumberExisted(phoneNumber);
        }

        public bool IsUserNameExisted(string userName)
        {
            return _unitOfWork.UserRepository.IsUserNameExisted(userName);
        }



        public async Task<IEnumerable<Users>> GetAvailableUsers(int projectId)
        {
            // Get all users and users assigned to the project
            var users = await GetAllUsersAsync();
            var projectUsers = await GetUsersByProjectId(projectId);

            // Filter users who are not part of the project
            var availableUsers = users.Where(u => !projectUsers.Any(pu => pu.Id == u.Id)).ToList();

            await _unitOfWork.CompleteAsync();

            return availableUsers;
        }

        public async Task<UserMembers> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var rolename = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            var result = new UserMembers
            {
                Id = user.Id,
                FullName = user.fullName,
                Roles = rolename,
                ImageUrl = user.PhotoUrl
            };

            return result;
        }
        public async Task<IEnumerable<UserMembers>> GetAllUsersWithProjectStatusAsync(int projectId)
        {

            var allUsers = (await GetAllUsersAsync()).ToList();


            var projectUsers = (await GetUsersByProjectId(projectId)).ToList();

            var result = new List<UserMembers>();

            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new UserMembers
                {
                    Id = user.Id,
                    FullName = user.fullName,
                    Roles = roles.FirstOrDefault(),
                    ImageUrl = user.PhotoUrl,
                });
            }

            await _unitOfWork.CompleteAsync();

            return result;
        }
    }
}
