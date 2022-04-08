using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manage.Repository.Entities
{
    public class Listing
    {
        [Key]
        public string Id { get; set; }

        public string Title { get; set; }

        public string ListingRef { get; set; } // Much more human friendly reference to use for end users

        public string PropertyId { get; set; }

        public bool ExistingProperty { get; set; } = false; // tracks if listing is for existing property or new

        public Property Property { get; set; } = null;

        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset DateListingStarts { get; set; } // date user set the listing to start on

        public DateTimeOffset DateListingExpires { get; set; } // date the listing will expire

        public string ListingDescription { get; set; } // specific description for this Ad

        // Status are:
        // 0: 'Incomplete' - 'You have started an Ad, but it is not yet complete'
        // 1: 'Pending Approval' - 'Your Ad is being checked. This takes a max of 48 hours'
        // 2: 'Live' - 'Your Ad is live online'
        // 3: 'Expires' - 'Your Ad was live but has not expired. You can renew by re-running the ad'
        // 4: 'Cancelled' - 'Your Ad was cancelled'

        public StatusCode StatusCode { get; set; } = StatusCode.Incomplete;

        public string Status { get; set; } // simply tracks the status of the ad

        public bool Complete { get; set; } = false; // Ad is complete - all fields are filled out properly
        public bool Active { get; set; } = false; // Ad is Live

        public string ProcessorAction { get; set; } // tells the Processor what to do

        public bool ReadyForPublishing { get; set; } = false;

        public bool IsPublishedLive { get; set; } = false;
        
        public bool Cancelled { get; set; } = false;

        public bool Relisted { get; set; } = false;

        public int RelistCount { get; set; } = 0;
        
        public bool Confirmed { get; set; } = false; // User has confirmed Ad to go live
        
        public DateTimeOffset ConfirmedDateTime { get; set; }
        

        public string ConfirmedByUserId { get; set; }

        public bool FirstEmailSent { get; set; } = false;
        
        // Only 1 type of listing can be active at once. For example, only 1 rent type can be active at once
        // Only 1 type of Buy listing can be active at once
        

        // Asking price
        public decimal Price { get; set; } = 0.00M;

        public string ListingType { get; set; } // rent, buy, auction etc

        public bool IsPremium { get; set; } = false; // default is standard

        public string ContactEmail { get; set; } // used to send emails to lister

        public string ContactNumber { get; set; } // can be landline or mobile

        public bool ShowNumber { get; set; } = false;
        
        // username who created to listing
        public string UserId { get; set; }
        
        public bool IsListedByAgent { get; set; } = false;

        public bool IsPrivateSeller { get; set; } = false; // default is not private seller

        public Agency Agency { get; set; } = null; // reference to Agency

        public string AgencyId { get; set; } // reference to Agency

        public Agent Agent { get; set; } = null;

        public string AgentId { get; set; }

        public List<ListingAction> ListingActions { get; set; }

    }

    public enum StatusCode
    {
        Incomplete = 0,
        Complete = 1,
        Active = 2,
        Expired = 3,
        Cancelled = 4
    }
}
