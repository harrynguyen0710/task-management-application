using Microsoft.EntityFrameworkCore;
using task_management.Data;
using task_management.IRepositories;
using task_management.Models;

namespace task_management.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<Users> GetUserById(string id)
        {
            return await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public bool IsEmailExisted(string email)
        {
            var isExisted = _context.Users.Any(m => m.Email == email);
            return isExisted;
            
        }
        public bool IsPhoneNumberExisted(string phoneNumber)
        {
            var isExisted = _context.Users.Any(m => m.PhoneNumber == phoneNumber);
            return isExisted;
        }

        public bool IsUserNameExisted(string userName)
        {
            var isExisted = _context.Users.Any(m => m.UserName == userName);
            return isExisted;
        }
    }
}
