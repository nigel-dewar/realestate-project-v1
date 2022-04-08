namespace Manage.API.Models.v1.Responses.Images.s3
{
    public class ListFilesResponse
    {
        public string BucketName { get; set; }

        public string Key { get; set; }

        public string Owner { get; set; }

        public long Size { get; set; }

    }
}
