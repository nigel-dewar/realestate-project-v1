namespace MessageBus.Constants
{
    public class EventBusConstants
    {
        public const string RabbitMqUri = "rabbitmq://rabbitmq/";
        public const string UserName = "guest";
        public const string Password = "guest";
        
        // Listings
        public const string PublishListingToMysqlCommandQueue = "publish.listing.mysql.command";
        public const string PublishListingToRethinkCommandQueue = "publish.listing.rethink.command";
        public const string PublishListingToElasticCommandQueue = "publish.listing.elastic.command";
        
        
        // Notifications
        public const string EmailConfirmationCommandQueue = "email.confirmation.command";
        public const string EmailWelcomeCommandQueue = "email.welcome.command";
        public const string SmsVerificationCommandQueue = "sms.verification.command";
        public const string EmailGenericCommand = "email.generic.command";
        
        
        // Test
        public const string TestPublishQueue = "test.publish.command";
        
        //Lookups
        public const string GetLookupsFromManageApiCommandQueue = "get.manage.lookups.command";
        
    }
}
