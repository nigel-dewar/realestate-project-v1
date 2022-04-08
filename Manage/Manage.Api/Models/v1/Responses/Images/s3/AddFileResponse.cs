using System.Collections.Generic;

namespace Manage.API.Models.v1.Responses.Images.s3
{
    public class AddFileResponse
    {
        public IList<string> PreSignedUrl { get; set; }
    }
}
