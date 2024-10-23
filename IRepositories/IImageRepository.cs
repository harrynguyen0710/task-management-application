namespace task_management.IRepositories
{
    public interface IImageRepository
    {
        string GetPhotoFileName(IFormFile file, string folder);
    }
}
