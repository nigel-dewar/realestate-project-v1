﻿namespace Identity.API.Models.v1.Requests
{
    public class UserLoginRequest
    {
        public string Email { get; set; }
            
        public string Password { get; set; }
    }
}