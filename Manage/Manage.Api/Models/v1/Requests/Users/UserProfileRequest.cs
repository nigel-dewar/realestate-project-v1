using System.Collections.Generic;

namespace Manage.API.Models.v1.Requests.Users
{
    public class UserProfileRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }
        
        public string Email { get; set; } // this field contains the email address to be used in liason. It allows person to set a different email address to their IDS user account

        public string MobileCountryCode { get; set; }
        public string MobileNumber { get; set; } // must have this before can create an Ad
        
        public string Bio { get; set; }

        public List<UserTypeRequest> UserTypes { get; set; }

        public bool AttemptConfirmation { get; set; }
    }
}