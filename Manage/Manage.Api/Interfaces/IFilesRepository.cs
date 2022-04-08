using System.Collections.Generic;
using System.Threading.Tasks;
using Manage.API.Models.v1.Responses.Images.s3;
using Microsoft.AspNetCore.Http;

namespace Manage.API.Interfaces
{
    public interface IFilesRepository
    {
        Task<AddFileResponse> UploadFiles(string bucketName, IList<IFormFile> formFiles);

        Task<string> UploadFile(IFormFile formFile);

        Task<IEnumerable<ListFilesResponse>> ListFiles(string bucketName);

        Task DownloadFile(string bucketName, string fileName);

        Task<DeleteFileResponse> DeleteFile(string fileName);
    }
}