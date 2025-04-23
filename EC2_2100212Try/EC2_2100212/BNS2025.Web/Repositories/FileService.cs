namespace BNS2025.Web.Repositories
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var webPath = _env.WebRootPath;
                var path = Path.Combine(webPath, "img\\", imageFileName);
                if (File.Exists(path) && imageFileName != null)
                {
                    File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            try
            {
                var webPath = _env.WebRootPath;
                var path = Path.Combine(webPath, "img");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtentions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtentions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtentions));
                    return new Tuple<int, string>(0, msg);
                }
                string uniqueString = Guid.NewGuid().ToString();
                var fileName = uniqueString + "_" + imageFile.FileName;
                var fileWithPath = Path.Combine(path, fileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, fileName);
            }
            catch (Exception)
            {
                return new Tuple<int, string>(0, "default.jpg");
            }
        }
    }
}
