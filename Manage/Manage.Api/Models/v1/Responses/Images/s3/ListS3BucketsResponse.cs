using System;

namespace Manage.API.Models.v1.Responses.Images.s3
{
    public class ListS3BucketsResponse
    {
        public string BucketName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
