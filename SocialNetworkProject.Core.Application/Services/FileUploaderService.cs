using Microsoft.AspNetCore.Http;
using SocialNetworkProject.Core.Application.Interfaces;

namespace SocialNetworkProject.Core.Application.Services
{
    public class FileUploaderService : IFileUploader
    {
        public string UploadFile(IFormFile file, string identifier, string subfolder, bool isEditMode = false, string existingPath = "")
        {
            if (isEditMode && file == null) return existingPath;
            if (file == null) return string.Empty;

            string basePath = Directory.GetCurrentDirectory();
            string fullPath = Path.Combine(basePath, "wwwroot", "images", subfolder, identifier);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            var guid = Guid.NewGuid();
            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{guid}{ext}";

            string fullFilePath = Path.Combine(fullPath, fileName);

            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode && !string.IsNullOrWhiteSpace(existingPath))
            {
                var oldFilePath = Path.Combine(basePath, "wwwroot", existingPath.TrimStart('/', '\\'));
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }

            return $"/images/{subfolder}/{identifier}/{fileName}".Replace("\\", "/");
        }

        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}