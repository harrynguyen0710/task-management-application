namespace task_management.IRepositories
{
    public interface IUserRepository
    {
        public bool IsEmailExisted(string email);
        public bool IsUserNameExisted(string userName);
        public bool IsPhoneNumberExisted(string phoneNumber);
    }
}
