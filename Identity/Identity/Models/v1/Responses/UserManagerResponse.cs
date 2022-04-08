using System.Collections.Generic;

namespace Identity.API.Models.v1.Responses
{
    public class UserManagerResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }
        
        public IEnumerable<string> Errors { get; set; }
    }
}