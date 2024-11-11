using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using task_management.Models;
using task_management.Services;

namespace task_management.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly UserService _userService;
        private readonly IdentityService _identityService;
        private readonly UserManager<Users> _userManager;   

        public ProfileController(ILogger<ProfileController> logger, UserService userService, 
            UserManager<Users> userManager, IdentityService identityService)
        {
            _logger = logger;
            _userService = userService;
            _userManager = userManager;
            _identityService = identityService; 
        }

        public async Task<IActionResult>Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userRoles = await _identityService.GetUserRole(user);
            var detailedUser = await _userService.GetDetailUser(user.Id);
            detailedUser.RoleNames = userRoles;
            detailedUser.User = user;
            return View(detailedUser);
        }
    }
}
