using Microsoft.AspNetCore.Mvc;
using task_management.Models;
using task_management.Services;

namespace task_management.Controllers
{
    public class AuthController : Controller
    {
        private readonly IdentityService _authService;
        public AuthController(IdentityService authService) 
        {
            _authService = authService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(Users user)
        {
            await _authService.CreateAccount(user);
            return RedirectToAction("Index", "Auth");
        }

    }
}
