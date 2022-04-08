namespace Manage.API.Models.v1.Responses.PostCodes
{
    public class PostCodeResponse
    {
        public string PostCode { get; set; }
        public string Locality { get; set; }
        public string State { get; set; }
        
        public string Long { get; set; }
        
        public string Lat { get; set; }
        
        public string Type { get; set; }
        
        public string Status { get; set; }
        public string Suburb { get; set; }
        public string Label { get; set; }
    }
}
