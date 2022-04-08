namespace Manage.API.Models.v1.Responses.Images
{
    public class CreateImageResponse
    {
        public string Id { get; set; }

        public string Url { get; set; }
        
        public bool IsMain { get; set; }
    }
}