using System;
using System.Collections.Generic;

namespace Manage.Repository.Entities
{
    public class UserProfile
    {
        public string Id { get; set; } // This is the userId from the IDS

        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; } // this field contains the email address to be used in liason. It allows person to set a different email address to their IDS user account

        public string MobileCountryCode { get; set; } // either AU or NZ
        
        public string MobileNumber { get; set; } // must have this before can create an Ad

        public bool MobileNumberChanged { get; set; } = false;

        public string PreviousMobileNumber { get; set; }

        public DateTimeOffset DateMobileNumberChanged { get; set; }

        public bool MobileConfirmed { get; set; } = false;

        public string MobileConfirmationCode { get; set; } // 6 digit numeric code required - this is SMS'd to user

        public DateTimeOffset MobileConfirmationCodeSet { get; set; }

        public int MobileConfirmationCodeFailedAttempts { get; set; } = 0;

        public DateTimeOffset DateTimeMobileConfirmed { get; set; }
        
        public string Bio { get; set; }

        public bool UserProfileComplete { get; set; } = false;
        
        public ICollection<UserProfileType> UserProfileTypes { get; set; } = new List<UserProfileType>();
        public ICollection<UserProperty> UserProperties { get; set; } = new List<UserProperty>();
        public ICollection<UserActivity> UserActivities { get; set; } = new List<UserActivity>();

        public ICollection<Image> Images { get; set; } = new List<Image>();
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

        public Agent Agent { get; set; }

    }
}
