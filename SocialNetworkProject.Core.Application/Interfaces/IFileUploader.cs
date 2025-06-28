using Microsoft.AspNetCore.Http;

namespace SocialNetworkProject.Core.Application.Interfaces
{
    public interface IFileUploader
    {
        string UploadFile(IFormFile file, string identifier, string subfolder, bool isEditMode = false, string existingPath = "");

        void DeleteFile(string filePath);
    }
}
