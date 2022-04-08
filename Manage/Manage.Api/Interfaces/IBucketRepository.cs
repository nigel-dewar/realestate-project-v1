using System.Collections.Generic;
using System.Threading.Tasks;
using Manage.API.Models.v1.Responses.Images.s3;

namespace Manage.API.Interfaces
{
    public interface IBucketRepository
    {
        Task<bool> DoesS3BucketExist(string bucketName);
        Task<CreateBucketResponse> CreateBucket(string bucketName);
        Task<IEnumerable<ListS3BucketsResponse>> ListBuckets();
        Task DeleteBucket(string bucketName);
    }
}