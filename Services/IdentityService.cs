﻿using Microsoft.AspNetCore.Identity;
using task_management.IRepositories;
using task_management.Models;
using task_management.ViewModels;

namespace task_management.Services
{
    public class IdentityService
    {
        const string FOLDER_NAME = "images";
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly EmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IImageRepository _imageRepository;

        public IdentityService(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager
            , SignInManager<Users> signInManager, EmailService emailService, IHttpContextAccessor httpContextAccessor
            , IImageRepository imageRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _imageRepository = imageRepository;
        }

        public async Task CreateAccount(Users user)
        {
            var password = Environment.GetEnvironmentVariable("password");
            user.organizationId = 1;
            user.PhotoUrl = _imageRepository.GetPhotoFileName(user.ProfilePhoto, FOLDER_NAME);

            user.UserName = user.Email;
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var userRole = await _userManager.FindByIdAsync(user.Id);
                var role = await _roleManager.FindByNameAsync("staff");
                await _userManager.AddToRoleAsync(userRole, role.Name);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                }
            }



        }

        public async Task<bool> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                return result.Succeeded;
            }
            return false;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IEnumerable<string>> GetUserRole(Users user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles;
        }


        public async Task<IEnumerable<UserRoles>> GetUsersWithRoles(List<Users> jointUsers)
        {
            var usersWithRoles = new List<UserRoles>();

            foreach (var user in jointUsers)
            { 
                var roles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add(new UserRoles
                {
                    Id = user.Id,
                    fullName = user.fullName,
                    Email = user.Email,
                    Roles = roles.ToList(),
                    ImageUrl = user.PhotoUrl
                });   
            }
            return usersWithRoles;
        }

      


        public async Task SendPasswordResetLink(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email), "Email cannot be blank.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Get scheme and host from HttpContext
            var request = _httpContextAccessor.HttpContext.Request;
            var resetLink = $"{request.Scheme}://{request.Host}/Auth/RenewPassword?userId={user.Id}&token={token}";

            // Send email
            await _emailService.SendEmailAsync(email, "Reset Password",
                $"Please reset your password by clicking here: <a href='{resetLink}'>link</a>");
        }
    }
}

