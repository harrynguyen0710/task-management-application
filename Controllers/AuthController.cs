using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using task_management.Models;
using task_management.Services;

namespace task_management.Controllers
{
    public class AuthController : Controller
    {
        private readonly IdentityService _authService;
        private readonly UserManager<Users> _userManager;
        private readonly EmailService _emailService;
        public AuthController(IdentityService authService, UserManager<Users> userManager, EmailService emailService)
        {
            _authService = authService;
            _userManager = userManager;
            _emailService = emailService;
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
                ModelState.AddModelError(string.Empty, "Email không hợp lệ.");
                return View();
            }

            // Tạo token đặt lại mật khẩu
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Tạo đường dẫn để đặt lại mật khẩu
            var resetLink = Url.Action("RenewPassword", "Auth", new { userId = user.Id, Token = token }, Request.Scheme);

            // Gửi email với liên kết đặt lại mật khẩu
            await _emailService.SendEmailAsync(email, "Reset Password", $"Vui lòng nhấn vào liên kết sau để đặt lại mật khẩu của bạn: <a href='{resetLink}'>Đặt lại mật khẩu</a>. Nếu bạn không yêu cầu thay đổi mật khẩu, vui lòng bỏ qua email này.");

            ViewBag.Message = "Reset password link has been sent to your email.";
            return RedirectToAction("Login", "Auth");
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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (await _authService.Login(email, password))
            {
                return RedirectToAction("Index", "Auth");
            }
            ModelState.AddModelError(string.Empty, "Mật khẩu hoặc Email không đúng. Vui lòng nhập lại!");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return RedirectToAction("Login");
        }
    }
}
