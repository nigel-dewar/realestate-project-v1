using Manage.API.Models.v1.Responses.Images.Cloudinary;
using Microsoft.AspNetCore.Http;

namespace Manage.API.Interfaces
{
    public interface IImageService
    {
        ImageUploadResponse AddImage(IFormFile file);
        

        string DeleteImage(string publicId);
    }
}