using task_management.Models;

namespace task_management.IRepositories
{
    public interface IUserRepository
    {
        public Task<Users> GetUserById(string id);
        public bool IsEmailExisted(string email);
        public bool IsUserNameExisted(string userName);
        public bool IsPhoneNumberExisted(string phoneNumber);
    }
}
