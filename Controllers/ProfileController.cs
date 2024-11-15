using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task_management.Data;
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
        private readonly ApplicationDbContext _context;

        public ProfileController(ILogger<ProfileController> logger, UserService userService, 
            UserManager<Users> userManager, IdentityService identityService, ApplicationDbContext context)
        {
            _logger = logger;
            _userService = userService;
            _userManager = userManager;
            _identityService = identityService; 
            _context = context;
            
        }

        public async Task<IActionResult>Index(string userId)
        {
            var user = new Users();
            if(userId == null)
            {
                user = await _userManager.GetUserAsync(User);
            } else
            {
                user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            }
            var userRoles = await _identityService.GetUserRole(user);
            var detailedUser = await _userService.GetDetailUser(user.Id);
            detailedUser.RoleNames = userRoles;
            detailedUser.User = user;
            return View(detailedUser);
        }

        

    }
}
