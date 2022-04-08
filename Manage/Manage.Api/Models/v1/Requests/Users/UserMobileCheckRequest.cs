namespace Manage.API.Models.v1.Requests.Users
{
    public class UserMobileCheckRequest
    {
        public string CountryCode { get; set; }
        public string MobileNumber { get; set; }
    }
}