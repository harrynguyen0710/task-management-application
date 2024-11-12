using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using task_management.Models;
using task_management.Services;

namespace task_management.Controllers
{
    public class AccountController : Controller
    {
        private readonly IdentityService _authService;
        private readonly UserManager<Users> _userManager;
        private readonly EmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserService _userSerivce;

        public AccountController(IdentityService authService, UserManager<Users> userManager,
            EmailService emailService, RoleManager<IdentityRole> roleManager, UserService userService)
        {
            _authService = authService;
            _userManager = userManager;
            _emailService = emailService;
            _roleManager = roleManager;
            _userSerivce = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CheckMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckMail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email.");
                return View();
            }

            // Create password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Create a path to reset password
            var resetLink = Url.Action("RenewPassword", "Account", new { userId = user.Id, Token = token }, Request.Scheme);

            // Send email with password reset link
            await _emailService.SendEmailAsync(email, "Reset Password", $"Please click on the following link to reset your password.: <a href='{resetLink}'>Reset Password</a>. If you did not request a password change, please ignore this email.");

            ViewBag.Message = "Reset password link has been sent to your email.";
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult RenewPassword(string userId, string token)
        {
            var model = new RenewPasswordModel
            {
                UserId = userId,
                Token = token
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RenewPassword(RenewPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [Authorize("Manager")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize("Manager")]
        [HttpPost]
        public async Task<IActionResult> Create(Users user)
        {
            var email = user.Email?.Trim();
            var username = user.UserName?.Trim();
            var phoneNumber = user.PhoneNumber?.Trim();

            if (_userSerivce.IsEmailExisted(email))
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(user);
            }

            if (_userSerivce.IsUserNameExisted(username))
            {
                ModelState.AddModelError("UserName", "Username already exists.");
                return View(user);
            }

            if (_userSerivce.IsPhoneNumberExisted(phoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "Phone number already exists.");
                return View(user);
            }

            await _authService.CreateAccount(user);
            return RedirectToAction("AllUsers", "Account");
        }
        [Authorize("Manager")]
        public async Task<IActionResult> AllUsers(int projectId)
        {
            var allUsers = await _userSerivce.GetAllUsersWithProjectStatusAsync(projectId);

            return View(allUsers);
        }

        public IActionResult Login(string ReturnUrl = null)
        {
            Console.WriteLine(ReturnUrl);
            ViewData["returnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string ReturnUrl = null)
        {
            if (await _authService.Login(email, password))
            {

                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }
                return RedirectToAction("Index", "Profile");
            }
            ModelState.AddModelError(string.Empty, "Password or Email is incorrect. Please re-enter!");
            return RedirectToAction("Account", "Login");
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return RedirectToAction("Login");
        }
    }
}