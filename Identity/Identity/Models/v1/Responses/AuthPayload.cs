using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Identity.API.Models.v1.Responses
{
    public class AuthResponse
    {

        [JsonProperty("access_token")]
        public string Token { get; set; }
        
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        
        [JsonProperty("access_token_expiration")]
        public DateTime AccessTokenExpiration { get; set; }
        
        public IEnumerable<string> Roles { get; set; }

        public int ExpiresIn { get; set; } = 0;

        public string Email { get; set; }
    }
}