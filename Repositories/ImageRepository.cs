using task_management.IRepositories;

namespace task_management.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHost;
        public ImageRepository(IWebHostEnvironment webHostEnvironment)
        {
            _webHost = webHostEnvironment;
        }
        public string GetPhotoFileName(IFormFile file, string folder)
        {
            string fileName = "null";
            if (file != null)
            {
                string uploadDir = Path.Combine(_webHost.WebRootPath, folder);
                fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return fileName;
        }
    }
}
