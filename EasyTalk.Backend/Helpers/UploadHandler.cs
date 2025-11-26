using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace EasyTalk.Backend.Helpers
{
    public class UploadHandler
    {
        private readonly long MaxSize = 5 * 1024 * 1024;
        private readonly string[] ValidExt = { ".jpg", ".jpeg", ".png" };
        private readonly string UploadFolder = "uploads/covers";

        public (bool Success, string Message) UploadFile(IFormFile file, IWebHostEnvironment env)
        {
            if (file == null || file.Length == 0)
                return (false, "File kosong.");

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (!ValidExt.Contains(ext))
                return (false, $"Format file harus salah satu: {string.Join(", ", ValidExt)}");

            if (file.Length > MaxSize)
                return (false, "Ukuran file maksimal 5MB");

            var wwwroot = env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot");
            var saveDir = Path.Combine(wwwroot, UploadFolder);
            Directory.CreateDirectory(saveDir);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(saveDir, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            file.CopyTo(stream);

            var url = $"/{UploadFolder}/{fileName}";
            return (true, url);
        }
    }
}
