namespace Identity.API.Models.v1.Requests
{
    public class UserRegisterRequest
    {
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}