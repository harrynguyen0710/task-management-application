﻿using Microsoft.AspNetCore.Identity;
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
                ModelState.AddModelError(string.Empty, "Invalid email.");
                return View();
            }

            // Create password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Create a path to reset password
            var resetLink = Url.Action("RenewPassword", "Auth", new { userId = user.Id, Token = token }, Request.Scheme);

            // Send email with password reset link
            await _emailService.SendEmailAsync(email, "Reset Password", $"Please click on the following link to reset your password.: <a href='{resetLink}'>Reset Password</a>. If you did not request a password change, please ignore this email.");

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
            ModelState.AddModelError(string.Empty, "Password or Email is incorrect. Please re-enter!");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return RedirectToAction("Login");
        }
    }
}
