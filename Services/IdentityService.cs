using task_management.Models;
using Microsoft.AspNetCore.Identity;

namespace task_management.Services
{
    public class IdentityService
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public IdentityService(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager) 
        {    
            _userManager = userManager; 
            _roleManager = roleManager;
        }

        public async Task CreateAccount(Users user)
        {
            var password = Environment.GetEnvironmentVariable("password");
            user.organizationId = 1;
            user.UserName = user.Email;
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded) 
            {
                var userRole = await _userManager.FindByIdAsync(user.Id);
                var role = await _roleManager.FindByNameAsync("staff");
                await _userManager.AddToRoleAsync(userRole, role.Name);
            } else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                }
            }
            
        }


    }
}
