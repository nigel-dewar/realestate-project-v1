using System;
using System.Collections.Generic;

namespace Manage.API.Models.v1.Responses.Users
{
    public class UserProfileResponse
    {
        public string Id { get; set; } // This is the userId from the IDS

        public DateTimeOffset DateCreated { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; } // this field contains the email address to be used in liason. It allows person to set a different email address to their IDS user account

        public string MobileCountryCode { get; set; }
        
        public string MobileNumber { get; set; } // must have this before can create an Ad

        public bool MobileConfirmed { get; set; } = false;

        public string Bio { get; set; }

        public bool UserProfileComplete { get; set; } = false;
        
        public List<UserTypeResponse> UserTypes { get; set; }
    }
}